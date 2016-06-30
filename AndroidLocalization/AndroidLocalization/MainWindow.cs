﻿//------------------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace AndroidLocalization
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using ViewModels;
    using Data;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("0e777183-4764-4eab-b877-98ca7dd6dea6")]
    public class MainWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow() : base(null)
        {
            this.Caption = "MainWindow";

            var mainViewModel = new MainViewModel(new Managers.LocalizationManager(new StringsFileLocator(), new StringsFileLoader(new StringsFileReader()), new StringsFileDataTableBuilder()));
            mainViewModel.Load(GetSolutionDirectory());

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new MainWindowControl(mainViewModel);
        }

        private string GetSolutionDirectory()
        {
            var service = (IVsSolution)GetService(typeof(SVsSolution));
            if (service == null) return string.Empty;
            string solutionDirectory, solutionFile, userOptsFile;
            service.GetSolutionInfo(out solutionDirectory, out solutionFile, out userOptsFile);
            return solutionDirectory;
        }
    }
}
