using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AndroidLocalization.Data
{
    public interface IStringsFileWriter
    {
        void WriteAll(XDocument document, StringsFile stringsFile);
    }

    public class StringsFileWriter : IStringsFileWriter
    {
        public void WriteAll(XDocument document, StringsFile stringsFile)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (stringsFile == null) throw new ArgumentNullException(nameof(stringsFile));
            var rootElement = new XElement("resources");
            foreach (var row in stringsFile.Rows)
                rootElement.Add(new XElement("string", new XAttribute("name", row.Key), row.Value));
            document.Add(rootElement);
        }
    }
}
