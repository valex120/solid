using System.Text;
using SmartConvertor.Models;
using SmartConvertor.Interfaces;

namespace SmartConvertor.Converters
{
    public class TxtToPdfConverter : IFileConverter
    {
        public bool Supports(PrintServiceSupportedFileType fileType) => fileType == PrintServiceSupportedFileType.Txt;

        public byte[] Convert(byte[] fileContent)
        {
            // dummy-конвертация TXT в PDF.
            string text = "TXT converted to PDF: " + Encoding.UTF8.GetString(fileContent);
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
