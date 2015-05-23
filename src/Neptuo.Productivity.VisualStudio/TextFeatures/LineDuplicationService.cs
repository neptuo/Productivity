using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.TextFeatures
{
    public class LineDuplicationService : DisposableBase, IVsService
    {
        private readonly DTE dte;
        private readonly OleMenuCommandService commandService;

        private MenuCommand downItem;
        private MenuCommand upItem;

        public LineDuplicationService(DTE dte, OleMenuCommandService commandService)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(commandService, "commandService");
            this.dte = dte;
            this.commandService = commandService;
            WireUpMenuCommands();
        }

        private void WireUpMenuCommands()
        {
            CommandID downCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.DuplicateLineDown);
            downItem = new MenuCommand(DuplicateLineDownCallback, downCommandID);
            commandService.AddCommand(downItem);

            CommandID upCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.DuplicateLineUp);
            upItem = new MenuCommand(DuplicateLineUpCallback, upCommandID);
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

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            commandService.RemoveCommand(downItem);
            commandService.RemoveCommand(upItem);
        }
    }
}
