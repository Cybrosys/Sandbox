﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.Data
{
    public interface IDataTableStringsFileMapper
    {
        List<StringsFile> Map(DataTable dataTable, List<StringsFile> stringsFiles);
    }

    public class DataTableStringsFileMapper : IDataTableStringsFileMapper
    {
        public List<StringsFile> Map(DataTable dataTable, List<StringsFile> stringsFiles)
        {
            if (dataTable == null) return null;
            if (stringsFiles == null) return null;
            stringsFiles.ForEach(stringsFile => Map(FindColumn(dataTable, stringsFile), stringsFile));
            return stringsFiles;
        }

        private DataColumn FindColumn(DataTable dataTable, StringsFile stringsFile)
        {
            string languageCodeOrDefault = string.IsNullOrWhiteSpace(stringsFile.LanguageCode) ? "Default" : stringsFile.LanguageCode;
            foreach (DataColumn column in dataTable.Columns)
            {
                if ((string)column.ExtendedProperties[nameof(stringsFile.LanguageCode)] == languageCodeOrDefault)
                    return column;
            }
            return null;
        }

        private void Map(DataColumn column, StringsFile stringsFile)
        {
            if (column == null || stringsFile == null) return;

            stringsFile.Rows.Clear();
            var rows = column.Table.Rows;
            foreach (DataRow row in rows)
            {
                var key = row[0]?.ToString();
                var value = row[column]?.ToString();
                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                    continue;
                
                stringsFile.Rows.Add(key, value);
            }
        }
    }
}
