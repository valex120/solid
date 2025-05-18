using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartConvertor.Interfaces;
using SmartConvertor.Converters;

namespace SmartConvertor.Tests
{
    [TestClass]
    public class ConvertersTests
    {
        [TestMethod]
        public void PdfFileConverter_ReturnsSameContent()
        {
            IFileConverter converter = new PdfFileConverter();
            byte[] input = Encoding.UTF8.GetBytes("PDF Content");
            byte[] output = converter.Convert(input);
            Assert.AreEqual(Encoding.UTF8.GetString(input), Encoding.UTF8.GetString(output));
        }

        [TestMethod]
        public void ImageToPdfConverter_ReturnsConvertedText()
        {
            IFileConverter converter = new ImageToPdfConverter();
            byte[] input = Encoding.UTF8.GetBytes("Image Content");
            byte[] output = converter.Convert(input);
            string result = Encoding.UTF8.GetString(output);
            Assert.IsTrue(result.Contains("Image converted to PDF"));
        }

        [TestMethod]
        public void TxtToPdfConverter_ReturnsConvertedText()
        {
            IFileConverter converter = new TxtToPdfConverter();
            byte[] input = Encoding.UTF8.GetBytes("TXT Content");
            byte[] output = converter.Convert(input);
            string result = Encoding.UTF8.GetString(output);
            Assert.IsTrue(result.Contains("TXT converted to PDF"));
        }

        [TestMethod]
        public void MarkdownToPdfConverter_ReturnsConvertedText()
        {
            IFileConverter converter = new MarkdownToPdfConverter();
            byte[] input = Encoding.UTF8.GetBytes("Markdown Content");
            byte[] output = converter.Convert(input);
            string result = Encoding.UTF8.GetString(output);
            Assert.IsTrue(result.Contains("Markdown converted to PDF"));
        }

        [TestMethod]
        public void JsonToPdfConverter_ReturnsConvertedText()
        {
            IFileConverter converter = new JsonToPdfConverter();
            byte[] input = Encoding.UTF8.GetBytes("{ \"key\": \"value\" }");
            byte[] output = converter.Convert(input);
            string result = Encoding.UTF8.GetString(output);
            Assert.IsTrue(result.Contains("JSON converted to PDF"));
        }
    }
}
