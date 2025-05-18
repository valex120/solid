using System.Text;
using SmartConvertor.Models;
using SmartConvertor.Interfaces;

namespace SmartConvertor.Converters
{
    public class MarkdownToPdfConverter : IFileConverter
    {
        public bool Supports(PrintServiceSupportedFileType fileType) => fileType == PrintServiceSupportedFileType.Markdown;

        public byte[] Convert(byte[] fileContent)
        {
            // dummy-конвертация Markdown в PDF.
            string text = "Markdown converted to PDF: " + Encoding.UTF8.GetString(fileContent);
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
