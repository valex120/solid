using SmartConvertor.Models;

namespace SmartConvertor.Interfaces
{
    public interface IFileConverter
    {
        bool Supports(PrintServiceSupportedFileType fileType);
        byte[] Convert(byte[] fileContent);
    }
}
