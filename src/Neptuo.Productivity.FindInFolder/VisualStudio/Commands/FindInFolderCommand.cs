using System;
using System.ComponentModel.Design;
using EnvDTE;

namespace Neptuo.Productivity.VisualStudio.Commands
{
    /// <summary>
    /// Binds 'Find In Folder' commands.
    /// </summary>
    internal sealed class FindInFolderCommand
    {
        private VsPackage package;
        private DTE dte;

        private FindInFolderCommand(VsPackage package, DTE dte, IMenuCommandService commandService)
        {
            this.package = package;
            this.dte = dte;

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
            throw new NotImplementedException();
        }

        #region Singleton

        private static FindInFolderCommand instance;

        /// <summary>
        /// Initializes new (singleton) instance if not already created.
        /// </summary>
        /// <param name="package">An instance of the package.</param>
        /// <param name="dte">A DTE.</param>
        /// <param name="commandService">A menu command service.</param>
        internal static void Initialize(VsPackage package, DTE dte, IMenuCommandService commandService)
        {
            if (instance == null)
                instance = new FindInFolderCommand(package, dte, commandService);
        }

        #endregion
    }
}
