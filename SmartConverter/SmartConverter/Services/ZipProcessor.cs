using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using SmartConvertor.Models;
using SmartConvertor.Interfaces;
using System.IO.Pipes;

namespace SmartConvertor.Services
{
    public class ZipProcessor : IZipProcessor
    {
        private readonly IFileTypeDetector _detector;
        private readonly IEnumerable<IFileConverter> _converters;
        private readonly IStampService _stampService;

        public ZipProcessor(IFileTypeDetector detector, IEnumerable<IFileConverter> converters, IStampService stampService)
        {
            _detector = detector;
            _converters = converters;
            _stampService = stampService;
        }

        public byte[] ProcessZip(byte[] zipContent, string documentIndefNumber, DateTime? documentRegisteredDateTime, Dictionary<string, string> certificate)
        {
            var entries = new Dictionary<string, byte[]>();
            using (var zipStream = new MemoryStream(zipContent))
            using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, false))
            {
                foreach (var entry in zipArchive.Entries)
                {
                    using (var entryStream = entry.Open())
                    using (var ms = new MemoryStream())
                    {
                        entryStream.CopyTo(ms);
                        entries[entry.FullName] = ms.ToArray();
                    }
                }
            }
            var validEntries = entries.Where(kv => _detector.Detect(kv.Key) != PrintServiceSupportedFileType.Unknown).ToList();

            if (validEntries.Count == 1)
            {
                var kv = validEntries.Single();
                var fileType = _detector.Detect(kv.Key);
                var converter = _converters.FirstOrDefault(c => c.Supports(fileType));
                if (converter == null)
                    throw new Exception("Нет конвертера для типа файла в архиве");
                var pdf = converter.Convert(kv.Value);
                StampInfo stampInfo = new StampInfo
                {
                    DocumentIndefNumber = documentIndefNumber,
                    DocumentRegisteredDateTime = documentRegisteredDateTime,
                    Certificate = certificate
                };
                return _stampService.AddStamp(pdf, stampInfo);
            }
            else if (validEntries.Count > 1)
            {
                using (var zipOutStream = new MemoryStream())
                {
                    using (var archiveOut = new ZipArchive(zipOutStream, ZipArchiveMode.Create, leaveOpen: true))
                    {
                        foreach (var item in validEntries)
                        {
                            var fileType = _detector.Detect(item.Key);
                            var converter = _converters.FirstOrDefault(c => c.Supports(fileType));
                            if (converter == null)
                                continue;
                            var pdf = converter.Convert(item.Value);
                            var stampInfo = new StampInfo
                            {
                                DocumentIndefNumber = documentIndefNumber,
                                DocumentRegisteredDateTime = documentRegisteredDateTime,
                                Certificate = certificate
                            };
                            pdf = _stampService.AddStamp(pdf, stampInfo);
                            var entryName = Path.GetFileNameWithoutExtension(item.Key) + "_ПечатнаяФорма.pdf";
                            var zipEntry = archiveOut.CreateEntry(entryName);
                            using (var entryStream = zipEntry.Open())
                            {
                                entryStream.Write(pdf, 0, pdf.Length);
                            }
                        }
                    }
                    return zipOutStream.ToArray();
                }
            }
            throw new Exception("Нет подходящих файлов для конвертации в архиве");
        }
    }
}
