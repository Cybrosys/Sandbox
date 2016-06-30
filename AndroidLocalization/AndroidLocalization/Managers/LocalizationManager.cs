using AndroidLocalization.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.Managers
{
    public class LocalizationManager
    {
        public List<StringsFile> FindAndLoadStringsFiles(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath)) throw new ArgumentNullException(nameof(directoryPath));

            //var xmlFiles = Directory.GetFiles(directoryPath, "*.xml", SearchOption.AllDirectories);

            throw new NotImplementedException();
        }
    }
}
