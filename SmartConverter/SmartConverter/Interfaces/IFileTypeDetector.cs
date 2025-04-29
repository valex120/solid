using SmartConvertor.Models;

namespace SmartConvertor.Interfaces
{
    public interface IFileTypeDetector
    {
        PrintServiceSupportedFileType Detect(string fileName);
    }
}
