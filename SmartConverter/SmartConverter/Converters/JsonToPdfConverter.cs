using System.Text;
using SmartConvertor.Models;
using SmartConvertor.Interfaces;

namespace SmartConvertor.Converters
{
    public class JsonToPdfConverter : IFileConverter
    {
        public bool Supports(PrintServiceSupportedFileType fileType) => fileType == PrintServiceSupportedFileType.Json;

        public byte[] Convert(byte[] fileContent)
        {
            // dummy-конвертация JSON в PDF.
            string text = "JSON converted to PDF: " + Encoding.UTF8.GetString(fileContent);
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
