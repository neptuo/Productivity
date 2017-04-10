using EnvDTE;
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
                        string message = $"'{line}' at '{index}'";
                        Console.WriteLine(message);

                        if (TryGetStringLiteralAt(line, index, out string path))
                        {
                            message = $"Path is '{path}'";
                            Console.WriteLine(path);

                            TryOpenStringLiteral(path);
                        }
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

        private bool TryGetStringLiteralAt(string content, int index, out string path)
        {
            int indexOfStartQuote = content.IndexOf("\"", 0, index);
            int indexOfEndQuote = content.IndexOf("\"", index);

            if (indexOfStartQuote >= 0 && indexOfEndQuote >= 0)
            {
                indexOfStartQuote++;
                path = content.Substring(indexOfStartQuote, indexOfEndQuote - indexOfStartQuote);
                return true;
            }

            path = null;
            return false;
        }

        private bool TryOpenStringLiteral(string literal)
        {
            string projectPath = Path.GetDirectoryName(dte.ActiveWindow.Project.FullName);
            string filePath = literal.Replace("~/", projectPath + @"\");

            ProjectItem item = dte.Solution.FindProjectItem(filePath);
            if (item == null)
                return false;

            Window window = item.Open();
            window.Activate();
            return true;
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
