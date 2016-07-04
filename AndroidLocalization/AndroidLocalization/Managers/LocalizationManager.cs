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
        private readonly IDataTableStringsFileMapper _mapper;
        private readonly IStringsFileSaver _saver;

        public LocalizationManager(IStringsFileLocator locator, IStringsFileLoader loader, IStringsFileDataTableBuilder builder, IDataTableStringsFileMapper mapper, IStringsFileSaver saver)
        {
            _locator = locator;
            _loader = loader;
            _builder = builder;
            _mapper = mapper;
            _saver = saver;
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

        public void SaveToFiles(DataTable dataTable, List<StringsFile> stringsFiles)
        {
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));
            if (stringsFiles == null) throw new ArgumentNullException(nameof(stringsFiles));
            if (stringsFiles.Count == 0) return;
            _mapper.Map(dataTable, stringsFiles);
            stringsFiles.ForEach(_saver.Save);
        }
    }
}
