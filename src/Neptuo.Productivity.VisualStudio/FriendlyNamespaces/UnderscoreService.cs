using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.FriendlyNamespaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.FriendlyNamespaces
{
    public class UnderscoreService
    {
        private readonly DTE dte;

        public UnderscoreService(DTE dte)
        {
            this.dte = dte;
        }

        public void WireUpMenuCommands(OleMenuCommandService commandService)
        {
            // Create the command for the menu item.
            CommandID menuCommandID = new CommandID(Constants.CommandSetGuid, Constants.CommandSet.UnderscoreRemover);
            MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            if (dte.ActiveDocument != null)
            {
                UnderscoreRemover underScore = new UnderscoreRemover();

                TextDocument doc = (TextDocument)(dte.ActiveDocument.Object("TextDocument"));
                var p = doc.StartPoint.CreateEditPoint();
                string s = p.GetText(doc.EndPoint);

                string newTextContent = underScore.FixNamespace(s);
                p.ReplaceText(doc.EndPoint, newTextContent, 0);
            }
        }
    }
}
