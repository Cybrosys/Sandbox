using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AndroidLocalization.Data
{
    public class StringsFile
    {
        public string FilePath { get; set; }
        public string LanguageCode { get; set; }
        public Dictionary<string, string> Rows { get; set; }
    }
}
