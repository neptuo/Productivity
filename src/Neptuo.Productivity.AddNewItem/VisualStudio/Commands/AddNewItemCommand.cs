using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.Views;
using System;
using System.ComponentModel.Design;

namespace Neptuo.Productivity.VisualStudio.Commands
{
    /// <summary>
    /// Binds 'Add New Item' commands.
    /// </summary>
    internal sealed class AddNewItemCommand
    {
        private VsPackage package;

        private AddNewItemCommand(VsPackage package, IMenuCommandService commandService)
        {
            this.package = package;

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
            AddNewItemWindow window = (AddNewItemWindow)package.FindToolWindow(typeof(AddNewItemWindow), 0, true);
            if (window != null && window.Frame != null)
            {
                IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }
        }

        #region Singleton

        private static AddNewItemCommand instance;

        /// <summary>
        /// Initializes new (singleton) instance if not already created.
        /// </summary>
        /// <param name="dte">A current package.</param>
        /// <param name="commandService">A menu command service.</param>
        internal static void Initialize(VsPackage package, IMenuCommandService commandService)
        {
            if (instance == null)
                instance = new AddNewItemCommand(package, commandService);
        }

        #endregion
    }
}
