using System;
using System.Text;
using SmartConvertor.Models;
using SmartConvertor.Interfaces;

namespace SmartConvertor.Services
{
    public class StampService : IStampService
    {
        public byte[] AddStamp(byte[] pdf, StampInfo stampInfo)
        {
            // Простой пример: добавляем информацию штампа в конец содержимого PDF.
            var stampText = new StringBuilder();
            stampText.AppendLine(new string('_', 141));
            stampText.AppendLine();
            stampText.AppendLine($"Документ {stampInfo.DocumentIndefNumber} от {stampInfo.DocumentRegisteredDateTime?.ToString("dd.MM.yyyy HH:mm:ss") ?? "-"} зарегистрирован. Документ подписан электронной подписью:");

            if (stampInfo.Certificate != null && stampInfo.Certificate.TryGetValue("Organization", out var org))
            {
                stampText.AppendLine($"{org} ОГРН {stampInfo.Certificate.GetValueOrDefault("OGRN")}");
            }
            if (stampInfo.ProxyData != null && stampInfo.ProxyData.TryGetValue("ProxyNum", out var proxy))
            {
                stampText.AppendLine($"Доверенность №{proxy}");
            }
            stampText.AppendLine();

            var stampBytes = Encoding.UTF8.GetBytes(stampText.ToString());
            byte[] result = new byte[pdf.Length + stampBytes.Length];
            Buffer.BlockCopy(pdf, 0, result, 0, pdf.Length);
            Buffer.BlockCopy(stampBytes, 0, result, pdf.Length, stampBytes.Length);
            return result;
        }
    }
}
