//------------------------------------------------------------------------------
// <copyright file="MainWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AndroidLocalization.Views
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using ViewModels;
    
    public partial class MainWindowControl : UserControl
    {
        public MainViewModel ViewModel => DataContext as MainViewModel;
        
        public MainWindowControl()
        {
            InitializeComponent();
        }
    }
}