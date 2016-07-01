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
    using System.Windows.Data;
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
            if (textColumn == null)
                return;

            var style = new Style(typeof(DataGridCell));
            var trigger = new DataTrigger() { Binding = textColumn.Binding };
            trigger.Value = "";
            trigger.Setters.Add(new Setter(BackgroundProperty, Brushes.Salmon));
            style.Triggers.Add(trigger);
            textColumn.CellStyle = style;
        }
    }
}