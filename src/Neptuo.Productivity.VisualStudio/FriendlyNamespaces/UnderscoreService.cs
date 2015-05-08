using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.FriendlyNamespaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using Thread = System.Threading.Thread;

namespace Neptuo.Productivity.VisualStudio.FriendlyNamespaces
{
    public class UnderscoreService
    {
        public const string KindPhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";

        private readonly DTE dte;
        private ProjectItemsEvents events;

        public UnderscoreService(DTE dte)
        {
            this.dte = dte;
        }

        public void WireUpMenuCommands(OleMenuCommandService commandService)
        {
            // Create the command for the menu item.
            CommandID menuCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.UnderscoreRemover);
            MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public void WireAutoEvents(ProjectItemsEvents events)
        {
            this.events = events;
            events.ItemAdded += OnItemAdded;
        }

        private void OnItemAdded(ProjectItem projectItem)
        {
            if (projectItem.Kind == KindPhysicalFile)
            {
                if (projectItem.Document != null)
                {
                    RunRemover(projectItem.Document);
                    projectItem.Save();
                }
                else
                {
                    ScheduleDelayRunRemover(projectItem);
                }
            }
        }

        private void ScheduleDelayRunRemover(ProjectItem projectItem)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1);
                OnItemAdded(projectItem);
            });
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            if (dte.ActiveDocument != null)
                RunRemover(dte.ActiveDocument);
        }

        private void RunRemover(Document document)
        {
            UnderscoreRemover underScore = new UnderscoreRemover();

            TextDocument doc = (TextDocument)(document.Object("TextDocument"));
            var p = doc.StartPoint.CreateEditPoint();
            string s = p.GetText(doc.EndPoint);

            string newTextContent = underScore.FixNamespace(s);
            p.ReplaceText(doc.EndPoint, newTextContent, 0);
        }
    }
}
