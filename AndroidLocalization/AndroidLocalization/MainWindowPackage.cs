//------------------------------------------------------------------------------
// <copyright file="MainWindowPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using AndroidLocalization.ViewModels;
using System.Windows;

namespace AndroidLocalization
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(MainWindow))]
    [Guid(MainWindowPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class MainWindowPackage : Package, IVsSolutionEvents
    {
        private IVsSolution2 _solution;
        private uint _solutionEventsCookie;

        /// <summary>
        /// MainWindowPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "ce98a3eb-5334-4b44-bab7-e1373fb3c765";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindowPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            MainWindowCommand.Initialize(this);
            base.Initialize();

            _solution = ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution)) as IVsSolution2;
            if (_solution != null)
                _solution.AdviseSolutionEvents(this, out _solutionEventsCookie);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_solution != null && _solutionEventsCookie != 0)
            {
                _solution.UnadviseSolutionEvents(_solutionEventsCookie);
                _solution = null;
            }
        }

        #endregion

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel) => VSConstants.S_OK;
        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            ToolWindowPane window = FindToolWindow(typeof(MainWindow), 0, false);
            if (window != null)
            {
                var mainWindow = (Views.MainWindowControl)window.Content;
                var viewModel = mainWindow.DataContext as MainViewModel;
                if (viewModel != null)
                {
                    if (viewModel.HasUnsavedChanges)
                    {
                        var result = MessageBox.Show("Do you want to save your changes?", "Android Localization", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Cancel)
                            pfCancel = 1;
                        else if (result == MessageBoxResult.Yes)
                            viewModel.SaveCommand.Execute(null);
                    }
                    if (pfCancel != 1)
                        viewModel.DirectoryPath = null;
                }
            }
            return VSConstants.S_OK;
        }
        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel) => VSConstants.S_OK;

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved) => VSConstants.S_OK;
        public int OnBeforeCloseSolution(object pUnkReserved) => VSConstants.S_OK;
        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy) => VSConstants.S_OK;

        public int OnAfterCloseSolution(object pUnkReserved) => VSConstants.S_OK;
        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy) => VSConstants.S_OK;
        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded) => VSConstants.S_OK;
        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            ToolWindowPane window = FindToolWindow(typeof(MainWindow), 0, false);
            if (window != null)
            {
                var mainWindow = (Views.MainWindowControl)window.Content;
                var viewModel = mainWindow.DataContext as MainViewModel;
                if (viewModel == null)
                {
                    viewModel = new MainViewModel();
                    mainWindow.DataContext = viewModel;
                }
                string solutionDirectory, solutionFile, userOptsFile;
                _solution.GetSolutionInfo(out solutionDirectory, out solutionFile, out userOptsFile);
                viewModel.DirectoryPath = solutionDirectory;
            }
            return VSConstants.S_OK;
        }
    }
}
