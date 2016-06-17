using AndroidLocalization.Models;
using AndroidLocalization.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AndroidLocalization.Loaders
{
    public class StringsFileLoader
    {
        private StringsFileParser _parser = new StringsFileParser();

        public StringsFile Load(string filePath)
        {
            var countryCode = GetCountryCodeFromFilePath(filePath);

            return new StringsFile
            {
                CountryCode = countryCode,
                Rows = _parser.Parse(XDocument.Load(filePath))
            };
        }

        private string GetCountryCodeFromFilePath(string filePath)
        {
            var directoryName = GetDirectoryNameFromFilePath(filePath).ToLower();

            if (!directoryName.StartsWith("values")) throw new ArgumentException(nameof(filePath));
            if (directoryName == "values") return string.Empty;
            return directoryName.Replace("values-", "");
        }

        private string GetDirectoryNameFromFilePath(string filePath)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            var directoryInfo = new DirectoryInfo(directoryPath);
            return directoryInfo.Name;
        }
    }
}
