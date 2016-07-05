//------------------------------------------------------------------------------
// <copyright file="MainWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AndroidLocalization.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using ViewModels;

    public partial class MainWindowControl : UserControl
    {
        public MainViewModel ViewModel => DataContext as MainViewModel;
        
        public MainWindowControl()
        {
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var textColumn = e.Column as DataGridTextColumn;
            if (textColumn == null) return;
            textColumn.Width = new DataGridLength(200, DataGridLengthUnitType.Pixel);

            var style = new Style(typeof(DataGridCell));
            textColumn.CellStyle = style;

            if (textColumn.Header.ToString() == "Key")
                ((Binding)textColumn.Binding).ValidationRules.Add(new IsNullOrWhiteSpaceValidationRule());

            var emptyStringTrigger = new DataTrigger { Binding = textColumn.Binding };
            emptyStringTrigger.Value = string.Empty;
            emptyStringTrigger.Setters.Add(new Setter(BackgroundProperty, new SolidColorBrush(Color.FromRgb(255, 255, 153))));
            style.Triggers.Add(emptyStringTrigger);
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var datagrid = (DataGrid)sender;
            if (e.Key != Key.Tab)
                return;
            var rowContainer = (DataGridRow)datagrid.ItemContainerGenerator.ContainerFromItem(datagrid.CurrentItem);
            if (rowContainer != null)
            {
                if (rowContainer.IsEditing)
                {
                    if (Validation.GetHasError(rowContainer))
                        e.Handled = true;
                    else
                        datagrid.CommitEdit(DataGridEditingUnit.Row, true);
                }
            }
        }

        // Causes some graphical bugs with the DataGrid. The new empty row template doesn't get added untill after several presses
        // and the data triggers aren't fired so the background coloring gets messed up.
        //private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    var dataGrid = (DataGrid)sender;
        //    if (e.Key != Key.Enter)
        //        return;

        //    var rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.CurrentItem);
        //    if (rowContainer != null)
        //    {
        //        if (rowContainer.IsEditing)
        //        {
        //            var selectedItem = dataGrid.SelectedItem;
        //            if (!dataGrid.CommitEdit())
        //                return;
        //            dataGrid.SelectedItem = selectedItem;
        //        }

        //        var presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
        //        int columnIndex = dataGrid.Columns.IndexOf(dataGrid.CurrentColumn);
        //        var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
        //        var request = new TraversalRequest(FocusNavigationDirection.Next);
        //        request.Wrapped = true;
        //        cell.MoveFocus(request);

        //        dataGrid.SelectedItem = dataGrid.CurrentItem;
        //        e.Handled = true;
        //        dataGrid.UpdateLayout();
        //    }
        //}

        //public static T GetVisualChild<T>(Visual parent) where T : Visual
        //{
        //    var child = default(T);
        //    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < numVisuals; i++)
        //    {
        //        var v = (Visual)VisualTreeHelper.GetChild(parent, i);
        //        child = v as T;
        //        if (child == null)
        //            child = GetVisualChild<T>(v);

        //        if (child != null)
        //            break;
        //    }
        //    return child;
        //}
    }

    public class IsNullOrWhiteSpaceValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return new ValidationResult(false, null);
            return ValidationResult.ValidResult;
        }
    }
}