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
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell.Settings;
using Neptuo.Productivity.VisualStudio.Options;
using Neptuo.Productivity.VisualStudio.TextFeatures;
using Neptuo.Productivity.VisualStudio.Builds;
using Neptuo.Productivity.VisualStudio.UI.Builds;
using Neptuo.PresentationModels.TypeModels;
using Neptuo.PresentationModels;

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
    [ProvideOptionPage(typeof(FeaturePage), MyConstants.Feature.MainCategory, MyConstants.Feature.GeneralPage, 0, 0, true)]
    [ProvideToolWindow(typeof(BuildHistoryWindow))]
    public sealed partial class VsPackage : Package
    {
        private UnderscoreService underscoreService;
        private LineDuplicationService lineDeplicationService;
        private static BuildService buildService;

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

            ServiceFactory.Initialize(dte);

            // Underscore namespace remover.
            ServiceFactory.VsServices.Add(c => c.IsUnderscoreNamespaceRemoverUsed, new UnderscoreServiceActivator(dte, commandService));

            // Line duplications.
            ServiceFactory.VsServices.Add(c => c.IsLineDuplicatorUsed, new LineDuplicationServiceActivator(dte, commandService));


            // Run services.
            VsServiceConfigurationUpdater updater = new VsServiceConfigurationUpdater(
                ServiceFactory.VsServices, 
                ServiceFactory.ConfigurationDefinition, 
                new DictionaryModelValueProvider()
            );
            updater.Update(new ReflectionModelValueProvider(ServiceFactory.Configuration));

            // Builds
            RegisterBuildWatchers(dte, dte.Events.BuildEvents, commandService);
        }

        #region BuildWatchers

        private void RegisterBuildWatchers(DTE dte, BuildEvents events, OleMenuCommandService commandService)
        {
            buildService = new BuildService(dte);

            // Wire build measures.
            buildService.WireUpBuildEvents(events);

            // Wire menu item.
            if (commandService != null)
            {
                CommandID commandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.BuildHistory);
                MenuCommand menuItem = new MenuCommand(BuildHistoryCallback, commandID);
                commandService.AddCommand(menuItem);
            }
        }

        private void BuildHistoryCallback(object sender, EventArgs e)
        {
            BuildHistoryWindow window = (BuildHistoryWindow)FindToolWindow(typeof(BuildHistoryWindow), 0, true);
            if (window != null && window.Frame != null)
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }
        }

        public static BuildHistoryViewModel HistoryViewModel()
        {
            return new BuildHistoryViewModel(buildService.History);
        }

        #endregion
    }
}
