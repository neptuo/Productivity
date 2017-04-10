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
    /// <summary>
    /// Binds 'Go To Source' commands.
    /// </summary>
    public class GoToSourceCommand
    {
        private VsPackage package;
        private DTE dte;

        public GoToSourceCommand(VsPackage package, DTE dte, IMenuCommandService commandService)
        {
            this.package = package;
            this.dte = dte;

            WireUpMenuCommands(commandService);
        }

        private void WireUpMenuCommands(IMenuCommandService commandService)
        {
            CommandID downCommandID = new CommandID(PackageGuids.CommandSet, PackageIds.GoToSource);
            OleMenuCommand downCommand = new OleMenuCommand(OnGoToSource, downCommandID);
            downCommand.BeforeQueryStatus += new EventHandler(OnBeforeQueryStatus);
            commandService.AddCommand(downCommand);
        }

        private void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand command = (OleMenuCommand)sender;
            command.Enabled = true;
        }

        private void OnGoToSource(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Singleton

        private static GoToSourceCommand instance;

        /// <summary>
        /// Initializes new (singleton) instance if not already created.
        /// </summary>
        /// <param name="package">An instance of the package.</param>
        /// <param name="dte">A DTE.</param>
        /// <param name="commandService">A menu command service.</param>
        internal static void Initialize(VsPackage package, DTE dte, IMenuCommandService commandService)
        {
            if (instance == null)
                instance = new GoToSourceCommand(package, dte, commandService);
        }

        #endregion
    }
}
