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
using System.Windows.Forms;
using Neptuo.Productivity.FriendlyNamespaces;
using EnvDTE;
using Neptuo.Productivity.VisualStudio.FriendlyNamespaces;

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
    [Guid(MyConstants.PackageString)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed partial class VsPackage : Package
    {
        private UnderscoreService underscoreService;

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Initialize services.
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            ProjectItemsEvents csharpProjectItemsEvents = (ProjectItemsEvents)dte.Events.GetObject("CSharpProjectItemsEvents");
            OleMenuCommandService commandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            
            // Underscore service
            underscoreService = new UnderscoreService(dte);
            underscoreService.WireAutoEvents(csharpProjectItemsEvents);
            if (commandService != null)
                underscoreService.WireUpMenuCommands(commandService);



            //CSharpProjectItemsEvents events = (ProjectItemsEventsClass)ServiceProvider.GlobalProvider.GetService(typeof(ProjectItemsEventsClass));
            dte.Events.DocumentEvents.DocumentOpened += DocumentEvents_DocumentOpened;
            dte.Events.BuildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
            dte.Events.BuildEvents.OnBuildDone += BuildEvents_OnBuildDone;
            dte.Events.SolutionEvents.ProjectAdded += SolutionEvents_ProjectAdded;
        }

        void SolutionEvents_ProjectAdded(Project project)
        {
            //if (project.ConfigurationManager != null)
            //    project.ConfigurationManager.AddPlatform("x64", "Any CPU", true);

            //MessageBox.Show("ProjectAdded: " + project.Name);
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

        #endregion

    }
}
