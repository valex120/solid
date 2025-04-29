using System.Text;
using SmartConvertor.Models;
using SmartConvertor.Interfaces;

namespace SmartConvertor.Converters
{
    public class ImageToPdfConverter : IFileConverter
    {
        public bool Supports(PrintServiceSupportedFileType fileType) =>
            fileType == PrintServiceSupportedFileType.Jpg ||
            fileType == PrintServiceSupportedFileType.Jpeg ||
            fileType == PrintServiceSupportedFileType.Png ||
            fileType == PrintServiceSupportedFileType.Tiff ||
            fileType == PrintServiceSupportedFileType.Tif ||
            fileType == PrintServiceSupportedFileType.Bmp;

        public byte[] Convert(byte[] fileContent)
        {
            // dummy-конвертация изображения в PDF.
            string text = "Image converted to PDF: " + Encoding.UTF8.GetString(fileContent);
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
