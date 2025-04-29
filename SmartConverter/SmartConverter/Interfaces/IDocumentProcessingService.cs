using System;
using System.Collections.Generic;

namespace SmartConvertor.Interfaces
{
    public interface IDocumentProcessingService
    {
        byte[] ProcessDocument(
            byte[] fileContent,
            string fileName,
            string documentIndefNumber,
            DateTime? documentRegisteredDateTime,
            bool throwErrorOnUnknownType,
            Dictionary<string, string> certificate = null,
            Dictionary<string, string> proxyData = null);
    }
}
