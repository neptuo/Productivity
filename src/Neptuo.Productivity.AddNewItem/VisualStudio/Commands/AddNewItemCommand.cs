using EnvDTE;
using System;
using System.ComponentModel.Design;

namespace Neptuo.Productivity.VisualStudio.Commands
{
    /// <summary>
    /// Binds 'Add New Item' commands.
    /// </summary>
    internal sealed class AddNewItemCommand
    {
        private DTE dte;

        private AddNewItemCommand(DTE dte, IMenuCommandService commandService)
        {
            this.dte = dte;

            WireUpMenuCommands(commandService);
        }

        private void WireUpMenuCommands(IMenuCommandService commandService)
        {
            CommandID commandId = new CommandID(PackageGuids.CommandSet, PackageIds.AddNewItem);
            MenuCommand command = new MenuCommand(OnExecute, commandId);
            commandService.AddCommand(command);
        }

        private void OnExecute(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Add New Item...");
        }

        #region Singleton

        private static AddNewItemCommand instance;

        /// <summary>
        /// Initializes new (singleton) instance if not already created.
        /// </summary>
        /// <param name="dte">A DTE.</param>
        /// <param name="commandService">A menu command service.</param>
        internal static void Initialize(DTE dte, IMenuCommandService commandService)
        {
            if (instance == null)
                instance = new AddNewItemCommand(dte, commandService);
        }

        #endregion
    }
}
