using AndroidLocalization.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.Managers
{
    public class LocalizationManager
    {
        private readonly IStringsFileLocator _locator;
        private readonly IStringsFileLoader _loader;
        private readonly IStringsFileDataTableBuilder _builder;

        public LocalizationManager(IStringsFileLocator locator, IStringsFileLoader loader, IStringsFileDataTableBuilder builder)
        {
            _locator = locator;
            _loader = loader;
            _builder = builder;
        }

        public DataTable CreateDataTable(List<StringsFile> stringsFiles)
        {
            if (stringsFiles == null) throw new ArgumentNullException(nameof(stringsFiles));
            return _builder.Build(stringsFiles);
        }

        public List<StringsFile> GetStringsFiles(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath)) throw new ArgumentNullException(nameof(directoryPath));
            var filePaths = _locator.GetFilePaths(directoryPath);
            var stringsFiles = filePaths.Select(_loader.Load).ToList();
            return stringsFiles;
        }
    }
}
