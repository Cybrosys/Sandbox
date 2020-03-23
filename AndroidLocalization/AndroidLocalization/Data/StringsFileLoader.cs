using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AndroidLocalization.Data
{
    public interface IStringsFileLoader
    {
        StringsFile Load(string filePath);
        Task<StringsFile> LoadAsync(string filePath);
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

        async public Task<StringsFile> LoadAsync(string filePath)
        {
            var loadTask = Task.Run(() => XDocument.Load(filePath));
            var languageCode = GetLanguageCodeFromFilePath(filePath);
            return new StringsFile
            {
                FilePath = filePath,
                LanguageCode = languageCode,
                Rows = _reader.ReadAll(await loadTask.ConfigureAwait(false))
            };
        }

        private string GetLanguageCodeFromFilePath(string filePath)
        {
            var directoryName = GetDirectoryNameFromFilePath(filePath);

            if (!directoryName.StartsWith("values")) throw new ArgumentException(nameof(filePath));
            if (directoryName == "values") return string.Empty;
            if (directoryName.StartsWith("values-b"))
                return directoryName.Replace("values-b+", "").Replace("+", "-");
            else
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
