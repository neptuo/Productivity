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
    internal class LineDuplicationCommand
    {
        private readonly VsPackage package;
        private readonly DTE dte;

        public LineDuplicationCommand(VsPackage package, DTE dte, IMenuCommandService commandService)
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
            CommandID downCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.DuplicateLineDown);
            OleMenuCommand downCommand = new OleMenuCommand(OnDuplicateLineDown, downCommandID);
            downCommand.BeforeQueryStatus += new EventHandler(OnBeforeQueryStatus);
            commandService.AddCommand(downCommand);

            CommandID upCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.DuplicateLineUp);
            OleMenuCommand upCommand = new OleMenuCommand(OnDuplicateLineUp, upCommandID);
            upCommand.BeforeQueryStatus += new EventHandler(OnBeforeQueryStatus);
            commandService.AddCommand(upCommand);;
        }

        private void OnBeforeQueryStatus(object sender, EventArgs e)
        {
            OleMenuCommand command = (OleMenuCommand)sender;
            command.Enabled = dte.ActiveDocument != null && dte.ActiveDocument.GetTextDocument() != null;
        } 

        private void OnDuplicateLineDown(object sender, EventArgs e)
        {
            if (dte.ActiveDocument != null)
            {
                TextDocument textDocument = dte.ActiveDocument.GetTextDocument();
                if (textDocument != null)
                {
                    LineDuplicator duplicator = new LineDuplicator(textDocument);
                    duplicator.DuplicateCurrentLineDown();
                }
            }
        }

        private void OnDuplicateLineUp(object sender, EventArgs e)
        {
            if (dte.ActiveDocument != null)
            {
                TextDocument textDocument = dte.ActiveDocument.GetTextDocument();
                if (textDocument != null)
                {
                    LineDuplicator duplicator = new LineDuplicator(textDocument);
                    duplicator.DuplicateCurrentLineUp();
                }
            }
        }

        #region Singleton

        private static LineDuplicationCommand instance;

        internal static void Initialize(VsPackage package, DTE dte, IMenuCommandService commandService)
        {
            if (instance == null)
                instance = new LineDuplicationCommand(package, dte, commandService);
        }

        #endregion
    }
}
