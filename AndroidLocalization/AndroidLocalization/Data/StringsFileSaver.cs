using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AndroidLocalization.Data
{
    public interface IStringsFileSaver
    {
        void Save(StringsFile stringsFile);
    }

    public class StringsFileSaver : IStringsFileSaver
    {
        private IStringsFileWriter _writer;

        public StringsFileSaver(IStringsFileWriter writer)
        {
            _writer = writer;
        }

        public void Save(StringsFile stringsFile)
        {
            if (stringsFile == null) throw new ArgumentNullException(nameof(stringsFile));
            if (string.IsNullOrWhiteSpace(stringsFile.FilePath)) throw new ArgumentNullException(nameof(stringsFile.FilePath));
            var document = new XDocument(new XDeclaration("1.0", "utf-8", null));
            _writer.WriteAll(document, stringsFile);
            document.Save(stringsFile.FilePath);
        }
    }
}
