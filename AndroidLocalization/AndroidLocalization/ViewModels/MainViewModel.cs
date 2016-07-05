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
        private bool _hasUnsavedChanges;

        public string DirectoryPath
        {
            set
            {
                _directoryPath = value;
                _stringsFiles = null;
                DataTable = null;
            }
        }

        public DataTable DataTable
        {
            get { return _dataTable; }
            set
            {
                if (_dataTable != null)
                {
                    _dataTable.RowChanged -= _dataTable_RowChanged;
                    _dataTable.RowDeleted -= _dataTable_RowDeleted;
                }

                if (Set(ref _dataTable, value))
                {
                    if (_dataTable != null)
                    {
                        _dataTable.RowChanged += _dataTable_RowChanged;
                        _dataTable.RowDeleted += _dataTable_RowDeleted;
                    }
                    HasUnsavedChanges = _dataTable?.GetChanges() != null;
                }
            }
        }

        public bool HasUnsavedChanges
        {
            get { return _hasUnsavedChanges; }
            private set { Set(ref _hasUnsavedChanges, value); }
        }

        private ICommand _refreshCommand;
        private ICommand _saveCommand;

        public ICommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new RelayCommand(async () => await LoadAsync(), () => !IsBusy && !string.IsNullOrWhiteSpace(_directoryPath)));
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(async () => await SaveAsync(), () => !IsBusy && _stringsFiles != null && _stringsFiles.Count > 0 && _dataTable != null));

        public MainViewModel()
        {
            _manager = new LocalizationManager(
                new StringsFileLocator(),
                new StringsFileLoader(new StringsFileReader()),
                new StringsFileDataTableBuilder(),
                new DataTableStringsFileMapper(),
                new StringsFileSaver(new StringsFileWriter()));
        }

        protected override void OnIsBusyChanged()
        {
            ((RelayCommand)RefreshCommand).RaiseCanExecuteChanged();
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        async private Task LoadAsync()
        {
            using (new BusyContext(this))
            {
                if (string.IsNullOrWhiteSpace(_directoryPath)) return;
                if (HasUnsavedChanges)
                {
                    var result = MessageBox.Show("Do you want to save your changes before continuing?", "Android Localization", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Cancel) return;
                    else if (result == MessageBoxResult.Yes)
                        await SaveAsync();
                }
                _stringsFiles = await _manager.GetStringsFilesAsync(_directoryPath);
                var dataTable = _manager.CreateDataTable(_stringsFiles);
                dataTable.AcceptChanges();
                DataTable = dataTable;
            }
        }

        async private Task SaveAsync()
        {
            using (new BusyContext(this))
            {
                _dataTable.AcceptChanges();
                await _manager.SaveToFilesAsync(_dataTable, _stringsFiles);
                HasUnsavedChanges = false;
            }
        }

        private void _dataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            HasUnsavedChanges = _dataTable.GetChanges() != null;
        }
        
        private void _dataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            HasUnsavedChanges = _dataTable.GetChanges() != null;
        }
    }
}
