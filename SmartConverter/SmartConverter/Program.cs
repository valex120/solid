using System;
using System.Collections.Generic;
using System.Text;
using SmartConvertor.Interfaces;
using SmartConvertor.Services;
using SmartConvertor.Converters;

namespace SmartConvertor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настраиваем зависимости
            IFileTypeDetector detector = new FileTypeDetector();

            List<IFileConverter> converters = new List<IFileConverter>()
            {
                new PdfFileConverter(),
                new ImageToPdfConverter(),
                new TxtToPdfConverter(),
                new MarkdownToPdfConverter(),
                new JsonToPdfConverter()
            };

            IStampService stampService = new StampService();
            IZipProcessor zipProcessor = new ZipProcessor(detector, converters, stampService);

            IDocumentProcessingService documentProcessor = new DocumentProcessingService(detector, converters, stampService, zipProcessor);

            // Пример обработки – для демонстрации используем PDF (конвертер просто возвращает исходное содержимое)
            byte[] fileContent = Encoding.UTF8.GetBytes("Sample PDF content");
            string fileName = "document.pdf";
            string documentIndefNumber = "DOC123";
            DateTime? documentRegisteredDateTime = DateTime.Now;
            Dictionary<string, string> certificate = new Dictionary<string, string>()
            {
                { "Organization", "Acme Inc." },
                { "OGRN", "123456789" }
            };

            try
            {
                byte[] result = documentProcessor.ProcessDocument(
                    fileContent,
                    fileName,
                    documentIndefNumber,
                    documentRegisteredDateTime,
                    throwErrorOnUnknownType: true,
                    certificate: certificate);

                Console.WriteLine("Processing completed. Result: ");
                Console.WriteLine(Encoding.UTF8.GetString(result));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing document: " + ex.Message);
            }
        }
    }
}
