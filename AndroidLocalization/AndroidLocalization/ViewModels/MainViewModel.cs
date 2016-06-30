using AndroidLocalization.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly LocalizationManager _manager;
        private DataTable _dataTable;
        public DataTable DataTable
        {
            get { return _dataTable; }
            set { SetProperty(ref _dataTable, value); }
        }

        public MainViewModel(LocalizationManager localizationManager)
        {
            _manager = localizationManager;
        }

        public bool Load(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath)) return false;
            var files = _manager.GetStringsFiles(directoryPath);
            if (files == null || files.Count == 0) return false;
            DataTable = _manager.CreateDataTable(files);
            return true;
        }
    }
}
