using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Commands
{
    /// <summary>
    /// Binds line duplcation commands.
    /// </summary>
    internal class LineDuplicationCommand
    {
        private readonly VsPackage package;
        private readonly DTE dte;

        private LineDuplicationCommand(VsPackage package, DTE dte, IMenuCommandService commandService)
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
            CommandID downCommandID = new CommandID(PackageGuids.CommandSet, PackageIds.DuplicateLineDown);
            OleMenuCommand downCommand = new OleMenuCommand(OnDuplicateLineDown, downCommandID);
            downCommand.BeforeQueryStatus += new EventHandler(OnBeforeQueryStatus);
            commandService.AddCommand(downCommand);

            CommandID upCommandID = new CommandID(PackageGuids.CommandSet, PackageIds.DuplicateLineUp);
            OleMenuCommand upCommand = new OleMenuCommand(OnDuplicateLineUp, upCommandID);
            upCommand.BeforeQueryStatus += new EventHandler(OnBeforeQueryStatus);
            commandService.AddCommand(upCommand);;
        }

        /// <summary>
        /// Enables/Disables duplication commands based on current text document existance.
        /// </summary>
        private void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand command = (OleMenuCommand)sender;
            command.Enabled = dte.ActiveDocument != null && dte.ActiveDocument.GetTextDocument() != null;
        } 

        private void OnDuplicateLineDown(object sender, EventArgs e)
        {
            RunLineDuplicator(duplicator => duplicator.DuplicateCurrentLineDown());
        }

        private void OnDuplicateLineUp(object sender, EventArgs e)
        {
            RunLineDuplicator(duplicator => duplicator.DuplicateCurrentLineUp());
        }

        /// <summary>
        /// Tries to create a <see cref="LineDuplicator"/> for current text document.
        /// If text document exists, runs <paramref name="handler"/>.
        /// </summary>
        /// <param name="handler">An action to execute if current text document exists.</param>
        private void RunLineDuplicator(Action<LineDuplicator> handler)
        {
            if (dte.ActiveDocument != null)
            {
                TextDocument textDocument = dte.ActiveDocument.GetTextDocument();
                if (textDocument != null)
                {
                    LineDuplicator duplicator = new LineDuplicator(textDocument);
                    handler(duplicator);
                }
            }
        }

        #region Singleton

        private static LineDuplicationCommand instance;

        /// <summary>
        /// Initializes new (singleton) instance if not already created.
        /// </summary>
        /// <param name="package">An instance of the package.</param>
        /// <param name="dte">A DTE.</param>
        /// <param name="commandService">A menu command service.</param>
        internal static void Initialize(VsPackage package, DTE dte, IMenuCommandService commandService)
        {
            if (instance == null)
                instance = new LineDuplicationCommand(package, dte, commandService);
        }

        #endregion
    }
}
