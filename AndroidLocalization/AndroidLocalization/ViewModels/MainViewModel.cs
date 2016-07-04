using AndroidLocalization.Data;
using AndroidLocalization.Managers;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AndroidLocalization.ViewModels
{
    // Flags can be retrieved with this http://www.geognos.com/api/en/countries/flag/SE.png

    public class MainViewModel : ViewModelBase
    {
        private readonly LocalizationManager _manager;
        private string _directoryPath;
        private List<StringsFile> _stringsFiles;
        private DataTable _dataTable;

        public string DirectoryPath
        {
            set
            {
                _directoryPath = value;
                _stringsFiles = null;
                DataTable = null;
                Load();
            }
        }

        public DataTable DataTable
        {
            get { return _dataTable; }
            set { Set(ref _dataTable, value); }
        }

        private ICommand _refreshCommand;
        private ICommand _saveCommand;

        public ICommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(Load, () => !IsBusy && !string.IsNullOrWhiteSpace(_directoryPath)));
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(Save, () => !IsBusy && _stringsFiles != null && _stringsFiles.Count > 0 && _dataTable != null));

        public MainViewModel()
        {
            _manager = new LocalizationManager(
                new StringsFileLocator(),
                new StringsFileLoader(new StringsFileReader()),
                new StringsFileDataTableBuilder(),
                new DataTableStringsFileMapper(),
                new StringsFileSaver(new StringsFileWriter()));
        }

        private void Load()
        {
            using (new BusyContext(this))
            {
                if (_dataTable != null && _dataTable.DataSet.HasChanges())
                {
                    if (MessageBox.Show("You have unsaved changes, continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        return;
                }

                if (string.IsNullOrWhiteSpace(_directoryPath)) return;
                _stringsFiles = _manager.GetStringsFiles(_directoryPath);
                DataTable = _manager.CreateDataTable(_stringsFiles);
            }
        }

        private void Save()
        {
            using (new BusyContext(this))
            {
                _dataTable.AcceptChanges();
                _manager.SaveToFiles(_dataTable, _stringsFiles);
            }
        }
    }
}
