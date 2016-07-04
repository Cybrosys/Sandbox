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

            if (textColumn.Header.ToString() == "Key")
                textColumn.FontWeight = FontWeights.Bold;

            var style = new Style(typeof(DataGridCell));
            textColumn.CellStyle = style;

            var emptyStringTrigger = new DataTrigger { Binding = textColumn.Binding };
            emptyStringTrigger.Value = string.Empty;
            emptyStringTrigger.Setters.Add(new Setter(BackgroundProperty, Brushes.IndianRed));
            style.Triggers.Add(emptyStringTrigger);
        }

        private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (e.Key != Key.Enter)
                return;
            
            var rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.CurrentItem);
            if (rowContainer != null)
            {
                if (rowContainer.IsEditing)
                {
                    var selectedItem = dataGrid.SelectedItem;
                    if (!dataGrid.CommitEdit())
                        return;
                    dataGrid.SelectedItem = selectedItem;
                }

                var presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                int columnIndex = dataGrid.Columns.IndexOf(dataGrid.CurrentColumn);
                var cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                var request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                cell.MoveFocus(request);

                dataGrid.SelectedItem = dataGrid.CurrentItem;
                e.Handled = true;
                dataGrid.UpdateLayout();
            }
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            var child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                    child = GetVisualChild<T>(v);

                if (child != null)
                    break;
            }
            return child;
        }
    }
}