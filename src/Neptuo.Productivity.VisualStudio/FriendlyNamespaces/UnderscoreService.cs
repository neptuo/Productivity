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
    /// <summary>
    /// Service integrating <see cref="UnderscoreRemover"/> into Visual Studio.
    /// </summary>
    public class UnderscoreService
    {
        public const string KindPhysicalFile = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";

        private readonly DTE dte;
        private ProjectItemsEvents events; // important to store events as field (to prevent garbage collection).

        public UnderscoreService(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
        }

        public void WireUpMenuCommands(OleMenuCommandService commandService)
        {
            Ensure.NotNull(commandService, "commandService");
            CommandID menuCommandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.UnderscoreRemover);
            MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public void WireAutoEvents(ProjectItemsEvents events)
        {
            Ensure.NotNull(events, "events");
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
            TextDocument textDocument = (TextDocument)(document.Object("TextDocument"));
            int position = textDocument.Selection.ActivePoint.AbsoluteCharOffset;

            EditPoint editPoint = textDocument.StartPoint.CreateEditPoint();
            string textContent = editPoint.GetText(textDocument.EndPoint);

            UnderscoreRemover remover = new UnderscoreRemover();
            string newTextContent = remover.FixNamespace(textContent);

            editPoint.ReplaceText(textDocument.EndPoint, newTextContent, 0);
            editPoint.MoveToAbsoluteOffset(position);
        }
    }
}
