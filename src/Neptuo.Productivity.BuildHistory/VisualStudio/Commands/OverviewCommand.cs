using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Commands
{
    public class OverviewCommand
    {
        private readonly VsPackage package;
        private readonly DTE dte;

        private OverviewCommand(VsPackage package, DTE dte, IMenuCommandService commandService)
        {
            Ensure.NotNull(package, "package");
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(commandService, "commandService");
            this.dte = dte;
            this.package = package;

            WireUpMenuCommands(commandService);
        }

        private void WireUpMenuCommands(IMenuCommandService commandService)
        {
            CommandID overviewCommandID = new CommandID(PackageGuids.CommandSet, PackageIds.BuildHistoryOverview);
            MenuCommand overviewCommand = new MenuCommand(OnOpenOverview, overviewCommandID);
            commandService.AddCommand(overviewCommand);
        }

        private void OnOpenOverview(object sender, EventArgs e)
        {
            
        }

        #region Singleton

        private static OverviewCommand instance;

        /// <summary>
        /// Initializes new (singleton) instance if not already created.
        /// </summary>
        /// <param name="package">An instance of the package.</param>
        /// <param name="dte">A DTE.</param>
        /// <param name="commandService">A menu command service.</param>
        internal static void Initialize(VsPackage package, DTE dte, IMenuCommandService commandService)
        {
            if (instance == null)
                instance = new OverviewCommand(package, dte, commandService);
        }

        #endregion
    }
}
