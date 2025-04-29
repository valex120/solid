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
    public class DocumentProcessingServiceTests
    {
        private IDocumentProcessingService _service;
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
                new ImageToPdfConverter(),
                new TxtToPdfConverter(),
                new MarkdownToPdfConverter(),
                new JsonToPdfConverter()
            };
            _stampService = new StampService();
            _zipProcessor = new ZipProcessor(_detector, _converters, _stampService);
            _service = new DocumentProcessingService(_detector, _converters, _stampService, _zipProcessor);
        }

        [TestMethod]
        public void ProcessDocument_WithPdf_ReturnsStampedPdf()
        {
            // При наличии pdf-файла PdfFileConverter возвращает исходное содержимое,
            // а StampService добавляет штамп с номером документа и данными сертификата.
            byte[] input = Encoding.UTF8.GetBytes("Test PDF Content");
            string fileName = "sample.pdf";
            string docNumber = "DOC001";
            DateTime now = DateTime.Now;
            var certificate = new Dictionary<string, string>
            {
                { "Organization", "Test Org" },
                { "OGRN", "0000" }
            };

            byte[] result = _service.ProcessDocument(input, fileName, docNumber, now, throwErrorOnUnknownType: true, certificate: certificate);
            string resultStr = Encoding.UTF8.GetString(result);

            Assert.IsTrue(resultStr.Contains("Test PDF Content"));
            Assert.IsTrue(resultStr.Contains("DOC001"));
            Assert.IsTrue(resultStr.Contains("Test Org"));
        }

        [TestMethod]
        public void ProcessDocument_WithTxt_ReturnsStampedPdf()
        {
            // Проверяем обработку текстового файла через TxtToPdfConverter
            byte[] input = Encoding.UTF8.GetBytes("Test TXT Content");
            string fileName = "document.txt";
            string docNumber = "DOC002";
            DateTime now = DateTime.Now;
            var certificate = new Dictionary<string, string>
            {
                { "Organization", "TXT Org" },
                { "OGRN", "1111" }
            };

            byte[] result = _service.ProcessDocument(input, fileName, docNumber, now, throwErrorOnUnknownType: false, certificate: certificate);
            string resultStr = Encoding.UTF8.GetString(result);

            Assert.IsTrue(resultStr.Contains("TXT converted to PDF"));
            Assert.IsTrue(resultStr.Contains("DOC002"));
            Assert.IsTrue(resultStr.Contains("TXT Org"));
        }

        [TestMethod]
        public void ProcessDocument_WithMarkdown_ReturnsStampedPdf()
        {
            // Проверяем обработку файла Markdown через MarkdownToPdfConverter
            byte[] input = Encoding.UTF8.GetBytes("Markdown Content");
            string fileName = "readme.md";
            string docNumber = "DOC003";
            DateTime now = DateTime.Now;
            var certificate = new Dictionary<string, string>
            {
                { "Organization", "Markdown Org" },
                { "OGRN", "2222" }
            };

            byte[] result = _service.ProcessDocument(input, fileName, docNumber, now, throwErrorOnUnknownType: false, certificate: certificate);
            string resultStr = Encoding.UTF8.GetString(result);

            Assert.IsTrue(resultStr.Contains("Markdown converted to PDF"));
            Assert.IsTrue(resultStr.Contains("DOC003"));
            Assert.IsTrue(resultStr.Contains("Markdown Org"));
        }

        [TestMethod]
        public void ProcessDocument_WithJson_ReturnsStampedPdf()
        {
            // Проверяем конвертацию JSON в PDF через JsonToPdfConverter
            byte[] input = Encoding.UTF8.GetBytes("{ \"key\": \"value\" }");
            string fileName = "data.json";
            string docNumber = "DOC004";
            DateTime now = DateTime.Now;
            var certificate = new Dictionary<string, string>
            {
                { "Organization", "JSON Org" },
                { "OGRN", "3333" }
            };

            byte[] result = _service.ProcessDocument(input, fileName, docNumber, now, throwErrorOnUnknownType: false, certificate: certificate);
            string resultStr = Encoding.UTF8.GetString(result);

            Assert.IsTrue(resultStr.Contains("JSON converted to PDF"));
            Assert.IsTrue(resultStr.Contains("DOC004"));
            Assert.IsTrue(resultStr.Contains("JSON Org"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ProcessDocument_UnknownType_ThrowsException()
        {
            // При неизвестном расширении и throwErrorOnUnknownType:true должно выбрасываться исключение
            byte[] input = Encoding.UTF8.GetBytes("Unknown content");
            string fileName = "file.unknown";
            _service.ProcessDocument(input, fileName, "DOC005", DateTime.Now, throwErrorOnUnknownType: true);
        }

        [TestMethod]
        public void ProcessDocument_WithProxyData_ReturnsStampedPdfContainingProxyInfo()
        {
            // Проверяем, что если переданы данные доверенности (proxyData),
            // они попадут в штамп PDF.
            byte[] input = Encoding.UTF8.GetBytes("Test Content with Proxy");
            string fileName = "document.pdf";
            string docNumber = "DOC006";
            DateTime now = DateTime.Now;
            var certificate = new Dictionary<string, string>
            {
                { "Organization", "Proxy Org" },
                { "OGRN", "4444" }
            };
            var proxyData = new Dictionary<string, string>
            {
                { "ProxyNum", "P-123" },
                { "StartDate", "01.01.2025" },
                { "EndDate", "31.12.2025" }
            };

            byte[] result = _service.ProcessDocument(input, fileName, docNumber, now, throwErrorOnUnknownType: false, certificate: certificate, proxyData: proxyData);
            string resultStr = Encoding.UTF8.GetString(result);

            Assert.IsTrue(resultStr.Contains("Test Content with Proxy"));
            Assert.IsTrue(resultStr.Contains("P-123"));
        }
    }
}
