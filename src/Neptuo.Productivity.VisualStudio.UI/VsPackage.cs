using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;
using Microsoft.Win32;
using Neptuo.Pipelines.Events.Handlers;
using Neptuo.PresentationModels;
using Neptuo.PresentationModels.TypeModels;
using Neptuo.Productivity.FriendlyNamespaces;
using Neptuo.Productivity.VisualStudio.Builds;
using Neptuo.Productivity.VisualStudio.FriendlyNamespaces;
using Neptuo.Productivity.VisualStudio.Misc;
using Neptuo.Productivity.VisualStudio.Options;
using Neptuo.Productivity.VisualStudio.TextFeatures;
using Neptuo.Productivity.VisualStudio.UI.Builds;
using Neptuo.Productivity.VisualStudio.UI.Builds.HistoryOverviews;
using Neptuo.Productivity.VisualStudio.UI.Options;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

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
    [ProvideToolWindow(typeof(QuickWindow))]
    public sealed partial class VsPackage : Package
    {
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

            ServiceFactory.Initialize((FeaturePage)GetDialogPage(typeof(FeaturePage)));

            // Underscore namespace remover.
            ServiceFactory.VsServices.Add(c => c.IsUnderscoreNamespaceRemoverUsed, new UnderscoreServiceActivator(dte, commandService));

            // Line duplications.
            ServiceFactory.VsServices.Add(c => c.IsLineDuplicatorUsed, new LineDuplicationServiceActivator(dte, commandService));

            // Builds.
            ServiceFactory.VsServices.Add(c => c.IsBuildHistoryUsed, new BuildServiceActivator(dte, ServiceFactory.EventDispatcher, commandService, BuildHistoryCallback));
            ServiceFactory.VsServices.Add(c => c.IsBuildCancelOnFirstErrorUsed, new BuildCancelServiceActivator(dte, ServiceFactory.Configuration));

            // Misc
            ServiceFactory.VsServices.Add(c => c.IsOpenStartPageOnSolutionCloseUsed, new StartPageServiceActivator(dte));

            // Run services.
            VsServiceConfigurationUpdater updater = new VsServiceConfigurationUpdater(
                ServiceFactory.VsServices, 
                ServiceFactory.ConfigurationDefinition, 
                new DictionaryModelValueProvider()
            );
            updater.Update(new ReflectionModelValueProvider(ServiceFactory.Configuration));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                ServiceFactory.Dispose();
        }

        #region BuildWatchers

        private void BuildHistoryCallback(object sender, EventArgs e)
        {
            QuickWindow window = (QuickWindow)FindToolWindow(typeof(QuickWindow), 0, true);
            if (window != null && window.Frame != null)
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }
        }

        #endregion
    }
}
