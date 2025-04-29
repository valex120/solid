using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartConvertor.Interfaces;
using SmartConvertor.Services;
using SmartConvertor.Converters;

namespace SmartConvertor.Tests
{
    [TestClass]
    public class ZipProcessorTests
    {
        private IFileTypeDetector _detector;
        private List<IFileConverter> _converters;
        private IStampService _stampService;
        private IZipProcessor _zipProcessor;

        [TestInitialize]
        public void Setup()
        {
            _detector = new FileTypeDetector();
            _converters = new List<IFileConverter>()
            {
                new PdfFileConverter(),
                new TxtToPdfConverter(),
                new MarkdownToPdfConverter(),
                new ImageToPdfConverter(),
                new JsonToPdfConverter()
            };
            _stampService = new StampService();
            _zipProcessor = new ZipProcessor(_detector, _converters, _stampService);
        }

        [TestMethod]
        public void ProcessZip_WithSingleValidFile_ReturnsStampedPdf()
        {
            // Создаём ZIP-архив с одним txt файлом
            byte[] zipBytes;
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    var entry = archive.CreateEntry("test.txt");
                    using (var entryStream = entry.Open())
                    using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                    {
                        writer.Write("ZIP TXT Content");
                    }
                }
                zipBytes = ms.ToArray();
            }

            byte[] result = _zipProcessor.ProcessZip(zipBytes, "DOC_ZIP_TEST", DateTime.Now, new Dictionary<string, string>
            {
                { "Organization", "OrgTest" },
                { "OGRN", "5555" }
            });
            string resultStr = Encoding.UTF8.GetString(result);
            Assert.IsTrue(resultStr.Contains("TXT converted to PDF") || resultStr.Contains("ZIP TXT Content"));
            Assert.IsTrue(resultStr.Contains("DOC_ZIP_TEST"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ProcessZip_WithNoValidFiles_ThrowsException()
        {
            // Создаём ZIP-архив с файлом неизвестного типа
            byte[] zipBytes;
            using (var ms = new MemoryStream())
            {
                using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    var entry = archive.CreateEntry("file.unknown");
                    using (var entryStream = entry.Open())
                    using (var writer = new StreamWriter(entryStream, Encoding.UTF8))
                    {
                        writer.Write("Dummy File");
                    }
                }
                zipBytes = ms.ToArray();
            }

            _zipProcessor.ProcessZip(zipBytes, "DOC_FAIL", DateTime.Now, new Dictionary<string, string>());
        }
    }
}
