using AndroidLocalization.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.Managers
{
    public class LocalizationManager
    {
        private readonly IStringsFileLoader _loader;

        public LocalizationManager(IStringsFileLoader loader)
        {
            _loader = loader;
        }

        public DataTable CreateDataTable(List<StringsFile> stringsFiles)
        {
            if (stringsFiles == null)
                throw new ArgumentNullException(nameof(stringsFiles));

            var keys = GetDistinctAndOrderedKeys(stringsFiles);
            var table = CreateDataTable(keys, stringsFiles);

            throw new NotImplementedException();
        }

        public List<StringsFile> GetStringsFiles(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath)) throw new ArgumentNullException(nameof(directoryPath));
            var filePaths = GetStringsXmlFilePathsFromPath(directoryPath);
            var stringsFiles = GetStringsFilesFromFilePaths(filePaths);
            return stringsFiles;
        }

        private string[] GetStringsXmlFilePathsFromPath(string directoryPath)
        {
            var potentialFilePaths = Directory.GetFiles(directoryPath, "*.xml", SearchOption.AllDirectories);
            return potentialFilePaths.Where(filePath => new DirectoryInfo(Path.GetDirectoryName(filePath)).Name.ToLower().StartsWith("values")).ToArray();
        }

        private List<StringsFile> GetStringsFilesFromFilePaths(IEnumerable<string> filePaths)
        {
            return filePaths.Select(_loader.Load).ToList();
        }

        private List<string> GetDistinctAndOrderedKeys(List<StringsFile> stringsFiles)
        {
            var result = new List<string>();
            stringsFiles.ForEach(file => result.AddRange(file.Rows.Keys));
            result = result.Distinct().OrderBy(value => value).ToList();
            return result;
        }
        
        private DataTable CreateDataTable(List<string> keys, List<StringsFile> stringsFiles)
        {
            var table = new DataTable();

            table.Columns.Add("Name", typeof(string)).Unique = true;



            return table;
        }
    }
}
