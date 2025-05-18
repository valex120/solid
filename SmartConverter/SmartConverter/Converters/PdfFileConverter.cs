using SmartConvertor.Models;
using SmartConvertor.Interfaces;

namespace SmartConvertor.Converters
{
    public class PdfFileConverter : IFileConverter
    {
        public bool Supports(PrintServiceSupportedFileType fileType) => fileType == PrintServiceSupportedFileType.Pdf;

        public byte[] Convert(byte[] fileContent)
        {
            // Если файл уже PDF – возвращаем его без изменений.
            return fileContent;
        }
    }
}
