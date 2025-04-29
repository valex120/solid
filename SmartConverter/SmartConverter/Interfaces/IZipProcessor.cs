using System;
using System.Collections.Generic;
using SmartConvertor.Models;

namespace SmartConvertor.Interfaces
{
    public interface IZipProcessor
    {
        byte[] ProcessZip(byte[] zipContent, string documentIndefNumber, DateTime? documentRegisteredDateTime, Dictionary<string, string> certificate);
    }
}
