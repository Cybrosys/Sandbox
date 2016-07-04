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
        List<string> GetFilePaths(string directoryPath);
    }

    public class StringsFileLocator : IStringsFileLocator
    {
        public List<string> GetFilePaths(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath)) throw new ArgumentNullException(nameof(directoryPath));
            var potentialFilePaths = Directory.GetFiles(directoryPath, "Strings.xml", SearchOption.AllDirectories);
            var validFilePaths = potentialFilePaths.Where(IsValidFilePath).ToList();
            return validFilePaths;
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
