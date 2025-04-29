using System;
using System.Collections.Generic;

namespace SmartConvertor.Models
{
    public class StampInfo
    {
        public string DocumentIndefNumber { get; set; }
        public DateTime? DocumentRegisteredDateTime { get; set; }
        public Dictionary<string, string> Certificate { get; set; }
        public Dictionary<string, string> ProxyData { get; set; }
    }
}
