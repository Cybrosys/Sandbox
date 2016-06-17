using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AndroidLocalization.Models
{
    public class StringsFile
    {
        public string CountryCode { get; set; }
        public List<Tuple<string, string>> Rows { get; set; }
    }
}
