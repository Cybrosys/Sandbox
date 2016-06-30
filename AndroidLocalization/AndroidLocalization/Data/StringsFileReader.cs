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
        List<Tuple<string, string>> ReadAll(XDocument document);
    }

    public class StringsFileReader : IStringsFileReader
    {
        public List<Tuple<string, string>> ReadAll(XDocument document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            var rows = from row in document.Descendants("string")
                       select Tuple.Create(row.Attribute("name").Value, row.Value);

            return rows.ToList();
        }
    }
}
