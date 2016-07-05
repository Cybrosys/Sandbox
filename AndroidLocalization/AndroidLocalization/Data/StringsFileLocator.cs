using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.Data
{
    public interface IStringsFileLocator
    {
        IEnumerable<string> GetFilePaths(string directoryPath);
    }

    public class StringsFileLocator : IStringsFileLocator
    {
        public IEnumerable<string> GetFilePaths(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath)) throw new ArgumentNullException(nameof(directoryPath));

            var filePaths = from filePath in Directory.EnumerateFiles(directoryPath, "Strings.xml", SearchOption.AllDirectories)
                            where IsValidFilePath(filePath)
                            select filePath;
            return filePaths;
        }

        private bool IsValidFilePath(string filePath)
        {
            var valuesDirectoryPath = Path.GetDirectoryName(filePath);
            if (!new DirectoryInfo(valuesDirectoryPath).Name.ToLower().StartsWith("values"))
                return false;

            var resourcesDirectoryPath = Path.Combine(valuesDirectoryPath, "../");
            return new DirectoryInfo(resourcesDirectoryPath).Name.ToLower() == "resources";
        }
    }
}
