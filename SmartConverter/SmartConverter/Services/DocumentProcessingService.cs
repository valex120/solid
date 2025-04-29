using System;
using System.Collections.Generic;
using System.Linq;
using SmartConvertor.Models;
using SmartConvertor.Interfaces;

namespace SmartConvertor.Services
{
    public class DocumentProcessingService : IDocumentProcessingService
    {
        private readonly IFileTypeDetector _detector;
        private readonly IEnumerable<IFileConverter> _converters;
        private readonly IStampService _stampService;
        private readonly IZipProcessor _zipProcessor;

        public DocumentProcessingService(
            IFileTypeDetector detector,
            IEnumerable<IFileConverter> converters,
            IStampService stampService,
            IZipProcessor zipProcessor)
        {
            _detector = detector;
            _converters = converters;
            _stampService = stampService;
            _zipProcessor = zipProcessor;
        }

        public byte[] ProcessDocument(
            byte[] fileContent,
            string fileName,
            string documentIndefNumber,
            DateTime? documentRegisteredDateTime,
            bool throwErrorOnUnknownType,
            Dictionary<string, string> certificate = null,
            Dictionary<string, string> proxyData = null)
        {
            var fileType = _detector.Detect(fileName);

            if (fileType == PrintServiceSupportedFileType.Unknown && fileName.EndsWith("zip", StringComparison.InvariantCultureIgnoreCase))
                return _zipProcessor.ProcessZip(fileContent, documentIndefNumber, documentRegisteredDateTime, certificate);

            if (fileType == PrintServiceSupportedFileType.Unknown && throwErrorOnUnknownType)
                throw new Exception($"Ошибка конвертации документа ({fileName}) в PDF.");

            var converter = _converters.FirstOrDefault(c => c.Supports(fileType));
            if (converter == null)
                throw new Exception("Нет конвертера для данного типа файла.");

            var pdf = converter.Convert(fileContent);
            if (pdf == null)
                throw new Exception("Ошибка при конвертации документа в PDF.");

            StampInfo stampInfo = new StampInfo
            {
                DocumentIndefNumber = documentIndefNumber,
                DocumentRegisteredDateTime = documentRegisteredDateTime,
                Certificate = certificate,
                ProxyData = proxyData
            };

            return _stampService.AddStamp(pdf, stampInfo);
        }
    }
}
