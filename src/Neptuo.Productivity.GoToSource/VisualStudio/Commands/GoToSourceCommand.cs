using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
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
            if (dte.ActiveDocument != null)
            {
                TextDocument textDocument = dte.ActiveDocument.GetTextDocument();
                if (textDocument != null)
                {
                    if (TryGetLineAt(textDocument, out string line, out int index))
                    {
                        GoToSourceService service = GetService<GoToSourceService>();
                        service.TryRun(line, index);
                    }
                }
            }
        }

        private bool TryGetLineAt(TextDocument textDocument, out string content, out int index)
        {
            EditPoint currentPoint = textDocument.CreateEditPoint();

            EditPoint startLine = textDocument.CreateEditPoint(textDocument.Selection.TopPoint);
            startLine.StartOfLine();

            // Create line end point.
            EditPoint endLine = textDocument.CreateEditPoint(textDocument.Selection.TopPoint);
            endLine.EndOfLine();

            // Get line text content.
            content = startLine.GetText(endLine);
            index = textDocument.Selection.TopPoint.LineCharOffset;
            return true;
        }

        private T GetService<T>()
            where T : class
        {
            IComponentModel componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
            T service = componentModel.DefaultExportProvider.GetExportedValue<T>();
            return service;
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
