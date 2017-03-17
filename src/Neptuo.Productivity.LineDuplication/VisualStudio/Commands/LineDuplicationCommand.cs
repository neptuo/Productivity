using EnvDTE;
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
            MenuCommand downItem = new MenuCommand(DuplicateLineDownCallback, downCommandID);
            commandService.AddCommand(downItem);

            CommandID upCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.DuplicateLineUp);
            MenuCommand upItem = new MenuCommand(DuplicateLineUpCallback, upCommandID);
            commandService.AddCommand(upItem);
        }

        private void DuplicateLineDownCallback(object sender, EventArgs e)
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

        private void DuplicateLineUpCallback(object sender, EventArgs e)
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
