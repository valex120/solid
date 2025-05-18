using System.IO;
using SmartConvertor.Models;
using SmartConvertor.Interfaces;

namespace SmartConvertor.Services
{
    public class FileTypeDetector : IFileTypeDetector
    {
        public PrintServiceSupportedFileType Detect(string fileName)
        {
            string ext = Path.GetExtension(fileName).TrimStart('.').ToLowerInvariant();

            return ext switch
            {
                "pdf" => PrintServiceSupportedFileType.Pdf,
                "jpg" => PrintServiceSupportedFileType.Jpg,
                "jpeg" => PrintServiceSupportedFileType.Jpeg,
                "png" => PrintServiceSupportedFileType.Png,
                "bmp" => PrintServiceSupportedFileType.Bmp,
                "tiff" => PrintServiceSupportedFileType.Tiff,
                "tif" => PrintServiceSupportedFileType.Tif,
                "txt" => PrintServiceSupportedFileType.Txt,
                "xlsx" => PrintServiceSupportedFileType.Xlsx,
                "xls" => PrintServiceSupportedFileType.Xlsx,
                "xlsm" => PrintServiceSupportedFileType.Xlsx,
                "docx" => PrintServiceSupportedFileType.Docx,
                "doc" => PrintServiceSupportedFileType.Doc,
                "rtf" => PrintServiceSupportedFileType.Rtf,
                "html" => PrintServiceSupportedFileType.Html,
                "htm" => PrintServiceSupportedFileType.Htm,
                "md" => PrintServiceSupportedFileType.Markdown,
                "json" => PrintServiceSupportedFileType.Json,
                _ => PrintServiceSupportedFileType.Unknown,
            };
        }
    }
}
