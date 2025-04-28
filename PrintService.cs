using System.IO.Compression;
using System.Text;

namespace Innotech31.Task
{
    public enum PrintServiceSupportedFileType
    {
        Jpg = 0,
        Bmp = 1,
        Png = 2,
        Tiff = 3,
        Txt = 4,
        Pdf = 5,
        Unknown = 6,
        Xlsx = 11,
        Docx = 12,
        Doc = 13,
        Xls = 14,
        Rtf = 15,
        Html = 16,
        Htm = 17,
        Jpeg = 18,
        Xlsm = 19,
        Tif = 20
    }

    public enum ImageFormat
    {
        Jpeg,
        Png,
        Tiff,
        Bmp
    }

    public static class PrintService
    {
        static byte[] ConvertInputFileToPdf(byte[] inputFileBytes, PrintServiceSupportedFileType inputFileType)
        {
            byte[] pdfFileBytes = null;
            switch (inputFileType)
            {
                case PrintServiceSupportedFileType.Pdf:
                    // PDF конвертировать не нужно - его напрямую в обработку отдаем
                    pdfFileBytes = inputFileBytes;
                    break;
                case PrintServiceSupportedFileType.Jpeg:
                case PrintServiceSupportedFileType.Jpg:
                case PrintServiceSupportedFileType.Bmp:
                case PrintServiceSupportedFileType.Png:
                case PrintServiceSupportedFileType.Tif:
                case PrintServiceSupportedFileType.Tiff:
                    pdfFileBytes = ConvertImageToPdf(inputFileBytes, inputFileType);
                    break;
                case PrintServiceSupportedFileType.Txt:
                    pdfFileBytes = ConvertTxtToPdf(inputFileBytes);
                    break;
                case PrintServiceSupportedFileType.Xls:
                case PrintServiceSupportedFileType.Xlsx:
                case PrintServiceSupportedFileType.Xlsm:
                    pdfFileBytes = ConvertXlsxToPdf(inputFileBytes);
                    break;
                case PrintServiceSupportedFileType.Doc:
                case PrintServiceSupportedFileType.Docx:
                    pdfFileBytes = ConvertDocxToPdf(inputFileBytes);
                    break;
                case PrintServiceSupportedFileType.Rtf:
                    pdfFileBytes = ConvertRtfToPdf(inputFileBytes);
                    break;
                case PrintServiceSupportedFileType.Htm:
                case PrintServiceSupportedFileType.Html:
                    pdfFileBytes = ConvertHtmlToPdf(inputFileBytes);
                    break;
                case PrintServiceSupportedFileType.Unknown:
                default:
                    pdfFileBytes = ConvertUnknownToPdf(inputFileBytes);
                    break;
            }

            return pdfFileBytes;
        }

        private static byte[] ConvertRtfToPdf(byte[] inputFileBytes)
        {
            return ConvertDocxToPdf(inputFileBytes);
        }

        static byte[] ConvertXlsxToPdf(byte[] inputFileBytes)
        {
            //специфика конвертации xlsx в pdf
            Console.WriteLine(inputFileBytes);
            throw new NotImplementedException();
        }

        static byte[] ConvertDocxToPdf(byte[] inputFileBytes)
        {
            //специфика конвертации docx в pdf
            Console.WriteLine(inputFileBytes);
            throw new NotImplementedException();
        }

        static byte[] ConvertUnknownToPdf(byte[] inputFileBytes)
        {
            //специфика конвертации неизвестного формата в pdf
            Console.WriteLine("{0}{1}", inputFileBytes);
            throw new NotImplementedException();
        }

        static byte[] ConvertTxtToPdf(byte[] inputFileBytes)
        {
            //специфика конвертации txt в pdf
            Console.WriteLine(inputFileBytes);
            throw new NotImplementedException();
        }

        static byte[] ConvertHtmlToPdf(byte[] inputFileBytes)
        {
            //специфика конвертации html в pdf
            Console.WriteLine(inputFileBytes);
            throw new NotImplementedException();
        }

        static byte[] ConvertImageToPdf(byte[] inputFileBytes, PrintServiceSupportedFileType inputFileType)
        {
            ImageFormat? supportedImageFileType = null;

            switch (inputFileType)
            {
                case PrintServiceSupportedFileType.Jpg:
                case PrintServiceSupportedFileType.Jpeg:
                    supportedImageFileType = ImageFormat.Jpeg;
                    break;
                case PrintServiceSupportedFileType.Png:
                    supportedImageFileType = ImageFormat.Png;
                    break;
                case PrintServiceSupportedFileType.Tiff:
                case PrintServiceSupportedFileType.Tif:
                    supportedImageFileType = ImageFormat.Tiff;
                    break;
                case PrintServiceSupportedFileType.Bmp:
                    supportedImageFileType = ImageFormat.Bmp;
                    break;
            }

            return ConvertSupportedImageToPdf(inputFileBytes, supportedImageFileType);
        }

        static byte[] ConvertSupportedImageToPdf(byte[] inputFileBytes, ImageFormat? fileType)
        {
            //специфика конвертации изображения в pdf
            Console.WriteLine("{0}{1}", inputFileBytes, fileType);
            throw new NotImplementedException();
        }

        static PrintServiceSupportedFileType DetectFileType(string fileName)
        {
            var detectedType = PrintServiceSupportedFileType.Unknown;
            var fileExtension = GetExtensionFromFileName(fileName);

            if (IsInputFilePdf(fileExtension))
                detectedType = PrintServiceSupportedFileType.Pdf;
            else if (IsInputFileJpg(fileExtension))
                detectedType = PrintServiceSupportedFileType.Jpg;
            else if (IsInputFileJpeg(fileExtension))
                detectedType = PrintServiceSupportedFileType.Jpeg;
            else if (IsInputFilePng(fileExtension))
                detectedType = PrintServiceSupportedFileType.Png;
            else if (IsInputFileBmp(fileExtension))
                detectedType = PrintServiceSupportedFileType.Bmp;
            else if (IsInputFileTiff(fileExtension))
                detectedType = PrintServiceSupportedFileType.Tiff;
            else if (IsInputFileTif(fileExtension))
                detectedType = PrintServiceSupportedFileType.Tif;
            else if (IsInputFileXlsx(fileExtension))
                detectedType = PrintServiceSupportedFileType.Xlsx;
            else if (IsInputFileXls(fileExtension))
                detectedType = PrintServiceSupportedFileType.Xls;
            else if (IsInputFileXlsm(fileExtension))
                detectedType = PrintServiceSupportedFileType.Xlsm;
            else if (IsInputFileDocx(fileExtension))
                detectedType = PrintServiceSupportedFileType.Docx;
            else if (IsInputFileDoc(fileExtension))
                detectedType = PrintServiceSupportedFileType.Doc;
            else if (IsInputFileTxt(fileExtension))
                detectedType = PrintServiceSupportedFileType.Txt;
            else if (IsInputFileRtf(fileExtension))
                detectedType = PrintServiceSupportedFileType.Rtf;
            else if (IsInputFileHtml(fileExtension))
                detectedType = PrintServiceSupportedFileType.Html;
            else if (IsInputFileHtm(fileExtension))
                detectedType = PrintServiceSupportedFileType.Htm;
            return detectedType;
        }

        static bool IsInputFileRtf(string fileExtension)
        {
            return fileExtension == "rtf";
        }

        static bool IsInputFilePdf(string fileExtension)
        {
            return fileExtension == "pdf";
        }

        static bool IsInputFileJpg(string fileExtension)
        {
            return fileExtension == "jpg";
        }

        static bool IsInputFileJpeg(string fileExtension)
        {
            return fileExtension == "jpeg";
        }

        static bool IsInputFilePng(string fileExtension)
        {
            return fileExtension == "png";
        }

        static bool IsInputFileBmp(string fileExtension)
        {
            return fileExtension == "bmp";
        }

        static bool IsInputFileTiff(string fileExtension)
        {
            return fileExtension == "tiff";
        }

        static bool IsInputFileTif(string fileExtension)
        {
            return fileExtension == "tif";
        }

        static bool IsInputFileTxt(string fileExtension)
        {
            return fileExtension == "txt";
        }

        static bool IsInputFileXlsx(string fileExtension)
        {
            return fileExtension == "xlsx";
        }

        static bool IsInputFileXls(string fileExtension)
        {
            return fileExtension == "xls";
        }

        static bool IsInputFileXlsm(string fileExtension)
        {
            return fileExtension == "xlsm";
        }

        static bool IsInputFileDocx(string fileExtension)
        {
            return fileExtension == "docx";
        }

        static bool IsInputFileDoc(string fileExtension)
        {
            return fileExtension == "doc";
        }

        static bool IsInputFileHtml(string fileExtension)
        {
            return fileExtension == "html";
        }

        static bool IsInputFileHtm(string fileExtension)
        {
            return fileExtension == "htm";
        }

        static string GetExtensionFromFileName(string fileName)
        {
            return Path.GetExtension(fileName).TrimStart('.').ToLower().Trim();
        }

        static byte[] AddWatermarkToPdf(byte[] pdfBytes, string additionalText)
        {
            //модификация уже сконвертиованного PDF, на каждую страницу в колонтитул добавляется информация об электронных подписях документа
            Console.WriteLine("{0}{1}", pdfBytes, additionalText);
            throw new NotImplementedException();
        }

        public static byte[] ProcessInputFile(
            byte[] inputFileBytes,
            string fileName,
            string documentIndefNumber,
            DateTime? documentRegisteredDateTime,
            bool throwErrorOnUnkwonType,
            Dictionary<string, string> certificate = null,
            Dictionary<string, string> proxyData = null)
        {
            // Определяем тип исходного файла
            var detectedFileType = DetectFileType(fileName);

            byte[] pdf = null;
            if (detectedFileType == PrintServiceSupportedFileType.Unknown && fileName.EndsWith("zip", StringComparison.InvariantCultureIgnoreCase))
            {
                var entries = new Dictionary<string, byte[]>();
                
                using (var zipStream = new MemoryStream(inputFileBytes))
                using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, false, Encoding.GetEncoding("cp866")))
                {
                    foreach (var zipArchiveEntry in zipArchive.Entries)
                    {
                        using (var memoryStream = new MemoryStream())
                        using (var entryStream = zipArchiveEntry.Open())
                        {
                            entryStream.CopyTo(memoryStream);
                            entries.Add(zipArchiveEntry.FullName, memoryStream.ToArray());
                        }
                    }
                }

                var countFileToConvert = entries.Where(el => DetectFileType(el.Key) != PrintServiceSupportedFileType.Unknown).Count();
                if (countFileToConvert == 1)
                {
                    var doc = entries.Where(el => DetectFileType(el.Key) != PrintServiceSupportedFileType.Unknown).FirstOrDefault();
                    pdf = ConvertInputFileToPdf(doc.Value, DetectFileType(doc.Key));
                }
                else if (countFileToConvert > 1)
                {
                    pdf = AddPdfFilesToZip(entries, documentIndefNumber, documentRegisteredDateTime, certificate);
                    return pdf;
                }
            }
            else
            {
                if (detectedFileType == PrintServiceSupportedFileType.Unknown && throwErrorOnUnkwonType)
                    throw new Exception($"Ошибка конвертации документа ({fileName}) при формировании печатной формы документа в PDF.");
                
                // Превращаем его в PDF
                pdf = ConvertInputFileToPdf(inputFileBytes, detectedFileType);
            }
			
			if (pdf == null)
                throw new Exception($"Ошибка при формировании печатной формы документа.");

            return AddStampToPdf(pdf, documentIndefNumber, documentRegisteredDateTime, certificate, proxyData);
        }

        public static byte[] AddStampToPdf(byte[] pdf, string documentIndefNumber, DateTime? documentRegisteredDateTime, Dictionary<string, string> certificate = null, Dictionary<string, string> proxyData = null)
        {
            var documentRegisteredDateTimeString = documentRegisteredDateTime.HasValue
                ? TimeZoneInfo.ConvertTime(documentRegisteredDateTime.Value, TimeZoneInfo.Utc, TimeZoneInfo.Local)
                    .ToString("dd.MM.yyyy HH:mm:ss")
                : "-";

            string organization = null;
            string surName = null;
            string givenName = null;
            string ogrn = null;
            string startDataSertificate = null;
            string endDataSertificate = null;
            string certificateSerialNumber = null;

            certificate?.TryGetValue("Organization", out organization);
            certificate?.TryGetValue("SurName", out surName);
            certificate?.TryGetValue("GivenName", out givenName);
            certificate?.TryGetValue("OGRN", out ogrn);
            certificate?.TryGetValue("NotBefore", out startDataSertificate);
            certificate?.TryGetValue("NotAfter", out endDataSertificate);
            certificate?.TryGetValue("CertificateSerialNumber", out certificateSerialNumber);

            string proxyNum = null;
            string proxyStartDate = null;
            string proxyEndDate = null;
            var proxyLine = "";
            if (proxyData != null)
            {
                proxyData.TryGetValue("Organization", out organization);
                proxyData.TryGetValue("OGRN", out ogrn);
                proxyData.TryGetValue("ProxyNum", out proxyNum);
                proxyData.TryGetValue("StartDate", out proxyStartDate);
                proxyData.TryGetValue("EndDate", out proxyEndDate);
                proxyLine = $"Доверенность №{ proxyNum}. Действительна c { proxyStartDate} по { proxyEndDate}.";
            }

            var separateLine = new string('_', 141);
            var watermarkText = new StringBuilder();
            watermarkText.AppendLine($"{separateLine}");
            watermarkText.AppendLine("");
            watermarkText.AppendLine($"Документ {documentIndefNumber} от {documentRegisteredDateTimeString} зарегистрирован. Документ подписан электронной подписью:   ");
            watermarkText.AppendLine("");
            
            if (string.IsNullOrWhiteSpace(organization) == false)
            {
                watermarkText.AppendLine($"{organization} ОГРН {ogrn}");
            }

            watermarkText.AppendLine($"{surName} {givenName}. {proxyLine}");
            watermarkText.AppendLine($"Серийный номер сертификата {certificateSerialNumber}. Действителен c {startDataSertificate} по {endDataSertificate} ");
            
            // Ставим штамп с номером документа
            byte[] watermarkedPdf = AddWatermarkToPdf(pdf, watermarkText.ToString());

            return watermarkedPdf;
        }

        private static byte[] AddPdfFilesToZip(Dictionary<string, byte[]> entries, string documentIndefNumber, DateTime? documentRegisteredDateTime, Dictionary<string, string> certificate)
        {
            using (var zipResponse = new MemoryStream())
            {
                using (var zipFileResponse = new ZipArchive(zipResponse, ZipArchiveMode.Create))
                {
                    foreach (var item in entries)
                    {
                        PrintServiceSupportedFileType entryDetectedFileType = DetectFileType(item.Key);
                        if (entryDetectedFileType != PrintServiceSupportedFileType.Unknown)
                        {
                            // Превращаем его в PDF
                            var pdf = ConvertInputFileToPdf(item.Value, entryDetectedFileType);
                            pdf = AddStampToPdf(pdf, documentIndefNumber, documentRegisteredDateTime, certificate);
                            var fileName = item.Key.Remove(item.Key.LastIndexOf('.'));
                            var file = zipFileResponse.CreateEntry(fileName + "_ПечатнаяФорма.pdf");
                            using (var stream = file.Open())
                            {
                                using (var fileMemoryStream = new MemoryStream(pdf))
                                {
                                    fileMemoryStream.WriteTo(stream);
                                }
                            }
                        }
                    }
                }

                return zipResponse.ToArray();
            }
        }
    }
}
