﻿using System;
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

            table.Columns.Add(new DataColumn("Key") { AllowDBNull = false, Unique = true });

            GetColumns(stringsFiles).ForEach(table.Columns.Add);
            keys.ForEach(key => table.Rows.Add(GetRowValuesForKey(key, stringsFiles)));

            return table;
        }

        private List<DataColumn> GetColumns(List<StringsFile> stringsFiles)
        {
            return stringsFiles.Select(file => new DataColumn(GetColumnNameFromCountryCode(file.CountryCode), typeof(string))).ToList();
        }

        private string GetColumnNameFromCountryCode(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return "Default";
            return new CultureInfo(countryCode).EnglishName;
        }

        private string[] GetRowValuesForKey(string key, List<StringsFile> stringsFiles)
        {
            return new[] { key }.Concat(stringsFiles.Select(file => file.Rows.ContainsKey(key) ? file.Rows[key] : null)).ToArray();
        }
    }
}
