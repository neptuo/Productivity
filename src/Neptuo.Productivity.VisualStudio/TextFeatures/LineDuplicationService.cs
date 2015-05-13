using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.TextFeatures
{
    public class LineDuplicationService
    {
        private readonly DTE dte;

        public LineDuplicationService(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
        }

        public void WireUpMenuCommands(OleMenuCommandService commandService)
        {
            Ensure.NotNull(commandService, "commandService");
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
    }
}
