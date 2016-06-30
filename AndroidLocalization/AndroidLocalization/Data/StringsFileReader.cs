using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AndroidLocalization.Data
{
    public interface IStringsFileReader
    {
        Dictionary<string, string> ReadAll(XDocument document);
    }

    public class StringsFileReader : IStringsFileReader
    {
        public Dictionary<string, string> ReadAll(XDocument document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            var rows = from row in document.Descendants("string")
                       select Tuple.Create(row.Attribute("name").Value, row.Value);

            return rows.ToDictionary(row => row.Item1, row => row.Item2);
        }
    }
}
