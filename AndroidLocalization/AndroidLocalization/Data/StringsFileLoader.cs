﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AndroidLocalization.Data
{
    public interface IStringsFileLoader
    {
        StringsFile Load(string filePath);
    }

    public class StringsFileLoader : IStringsFileLoader
    {
        private IStringsFileReader _reader;

        public StringsFileLoader(IStringsFileReader reader)
        {
            _reader = reader;
        }

        public StringsFile Load(string filePath)
        {
            var languageCode = GetLanguageCodeFromFilePath(filePath);

            return new StringsFile
            {
                FilePath = filePath,
                LanguageCode = languageCode,
                Rows = _reader.ReadAll(XDocument.Load(filePath))
            };
        }

        private string GetLanguageCodeFromFilePath(string filePath)
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