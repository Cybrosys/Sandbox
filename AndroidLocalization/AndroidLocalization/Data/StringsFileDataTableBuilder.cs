using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.Data
{
    public interface IStringsFileDataTableBuilder
    {
        DataTable Build(List<StringsFile> stringsFiles);
    }

    public class StringsFileDataTableBuilder : IStringsFileDataTableBuilder
    {
        public DataTable Build(List<StringsFile> stringsFiles)
        {
            if (stringsFiles == null) throw new ArgumentNullException(nameof(stringsFiles));
            var keys = GetDistinctAndOrderedKeys(stringsFiles);
            var table = CreateDataTable(keys, stringsFiles);
            return table;
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
            table.Columns.Add(new DataColumn("Key", typeof(string), string.Empty) { AllowDBNull = false, Unique = true });
            GetColumns(stringsFiles).ForEach(table.Columns.Add);
            keys.ForEach(key => table.Rows.Add(GetRowValuesForKey(key, stringsFiles)));
            return table;
        }

        private List<DataColumn> GetColumns(List<StringsFile> stringsFiles)
        {
            return stringsFiles.Select(file =>
            {
                var nameOrDefault = GetNameFromLanguageCode(file.LanguageCode);
                var languageCodeOrDefault = string.IsNullOrWhiteSpace(file.LanguageCode) ? nameOrDefault : file.LanguageCode;

                var column = new DataColumn
                {
                    Caption = nameOrDefault,
                    //ColumnName = nameOrDefault,
                    DataType = typeof(string),
                    DefaultValue = string.Empty
                };
                column.ExtendedProperties.Add(nameof(file.LanguageCode), languageCodeOrDefault);
                return column;
            }).ToList();
        }

        private string GetNameFromLanguageCode(string languageCode)
        {
            if (string.IsNullOrWhiteSpace(languageCode)) return "Default";
            return $"{new CultureInfo(languageCode).EnglishName} [{languageCode}]";
            //return new CultureInfo(languageCode).EnglishName;
        }

        private string[] GetRowValuesForKey(string key, List<StringsFile> stringsFiles)
        {
            return new[] { key }.Concat(stringsFiles.Select(file => file.Rows.ContainsKey(key) ? file.Rows[key] : string.Empty)).ToArray();
        }
    }
}
