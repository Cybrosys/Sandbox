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
            var validFilePaths = potentialFilePaths.Where(filePath => new DirectoryInfo(Path.GetDirectoryName(filePath)).Name.ToLower().StartsWith("values")).ToList();
            return validFilePaths;
        }
    }
}
