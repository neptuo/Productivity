using System;
using System.ComponentModel.Design;
using EnvDTE;
using System.IO;

namespace Neptuo.Productivity.VisualStudio.Commands
{
    /// <summary>
    /// Binds 'Find In Folder' commands.
    /// </summary>
    internal sealed class FindInFolderCommand
    {
        private DTE dte;
        private readonly FindInFolderService service;

        private FindInFolderCommand(DTE dte, IMenuCommandService commandService, FindInFolderService service)
        {
            this.dte = dte;
            this.service = service;

            WireUpMenuCommands(commandService);
        }

        private void WireUpMenuCommands(IMenuCommandService commandService)
        {
            CommandID commandId = new CommandID(PackageGuids.CommandSet, PackageIds.FindInFolder);
            MenuCommand command = new MenuCommand(OnExecute, commandId);
            commandService.AddCommand(command);
        }

        private void OnExecute(object sender, EventArgs e)
        {
            if (dte.SelectedItems.Count == 1)
            {
                SelectedItem item = dte.SelectedItems.Item(1);
                string filePath = null;
                if (item.ProjectItem != null)
                    filePath = item.ProjectItem.FileNames[0];
                else if (item.Project != null)
                    filePath = Path.GetDirectoryName(item.Project.FileName);
                else if(dte.Solution != null)
                    filePath = "Entire Solution";

                if (filePath != null)
                    service.Find(filePath);
            }
        }

        #region Singleton

        private static FindInFolderCommand instance;

        /// <summary>
        /// Initializes new (singleton) instance if not already created.
        /// </summary>
        /// <param name="dte">A DTE.</param>
        /// <param name="commandService">A menu command service.</param>
        /// <param name="serviceGetter">A factory for business service.</param>
        internal static void Initialize(DTE dte, IMenuCommandService commandService, Func<FindInFolderService> serviceGetter)
        {
            if (instance == null)
                instance = new FindInFolderCommand(dte, commandService, serviceGetter());
        }

        #endregion
    }
}
