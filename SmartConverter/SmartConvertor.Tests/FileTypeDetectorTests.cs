using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartConvertor.Interfaces;
using SmartConvertor.Services;
using SmartConvertor.Models;

namespace SmartConvertor.Tests
{
    [TestClass]
    public class FileTypeDetectorTests
    {
        private IFileTypeDetector _detector;

        [TestInitialize]
        public void Setup()
        {
            _detector = new FileTypeDetector();
        }

        [TestMethod]
        public void Detect_ReturnsPdf_ForPdfExtension()
        {
            var result = _detector.Detect("document.pdf");
            Assert.AreEqual(PrintServiceSupportedFileType.Pdf, result);
        }

        [TestMethod]
        public void Detect_ReturnsJpg_ForJpgExtension()
        {
            var result = _detector.Detect("image.JPG");
            Assert.IsTrue(result == PrintServiceSupportedFileType.Jpg || result == PrintServiceSupportedFileType.Jpeg);
        }

        [TestMethod]
        public void Detect_ReturnsMarkdown_ForMdExtension()
        {
            var result = _detector.Detect("readme.md");
            Assert.AreEqual(PrintServiceSupportedFileType.Markdown, result);
        }

        [TestMethod]
        public void Detect_ReturnsUnknown_ForUnsupportedExtension()
        {
            var result = _detector.Detect("file.abc");
            Assert.AreEqual(PrintServiceSupportedFileType.Unknown, result);
        }
    }
}
