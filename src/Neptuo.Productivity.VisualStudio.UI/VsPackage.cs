using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Composition;
using EnvDTE;
using EnvDTE80;
using System.Windows.Forms;

namespace Neptuo.Productivity.VisualStudio.UI
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(Constants.PackageString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed partial class VsPackage : Package
    {
        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            //CSharpProjectItemsEvents events = (ProjectItemsEventsClass)ServiceProvider.GlobalProvider.GetService(typeof(ProjectItemsEventsClass));

            ProjectItemsEvents csharpProjectItemsEvents = (ProjectItemsEvents)dte.Events.GetObject("CSharpProjectItemsEvents");

            csharpProjectItemsEvents.ItemAdded += SolutionItemsEvents_ItemAdded;
            dte.Events.DocumentEvents.DocumentOpened += DocumentEvents_DocumentOpened;
            dte.Events.BuildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
            dte.Events.BuildEvents.OnBuildDone += BuildEvents_OnBuildDone;
            dte.Events.SolutionEvents.ProjectAdded += SolutionEvents_ProjectAdded;

            //foreach (Project project in dte.Solution.Projects)
            //{
            //    project.
            //}

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(Constants.CommandSet1Guid, Constants.CommandSet1.Command1);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }

        void SolutionEvents_ProjectAdded(Project project)
        {
            if (project.ConfigurationManager != null)
                project.ConfigurationManager.AddPlatform("x64", "Any CPU", true);

            MessageBox.Show("ProjectAdded: " + project.Name);
        }

        private Stopwatch timer = new Stopwatch();

        void BuildEvents_OnBuildBegin(vsBuildScope scope, vsBuildAction action)
        {
            timer.Start();
        }

        void BuildEvents_OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            timer.Stop();
            MessageBox.Show(String.Format("Build tooked: {0}ms.", timer.ElapsedMilliseconds));
        }

        void DocumentEvents_DocumentOpened(Document document)
        {
            MessageBox.Show("DocumentOpened: " + document.FullName);
        }

        void SolutionItemsEvents_ItemAdded(ProjectItem projectItem)
        {
            MessageBox.Show("ItemAddded: " + projectItem.Name);
            //throw new NotImplementedException();
        }
        #endregion


        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            // Show a Message Box to prove we were here
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            int result;
            ErrorHandler.ThrowOnFailure(
                uiShell.ShowMessageBox(
                    0,
                    ref clsid,
                    "Productivity",
                    string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.ToString()),
                    string.Empty,
                    0,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                    OLEMSGICON.OLEMSGICON_INFO,
                    0,        // false
                    out result
                )
            );
        }
    }
}
