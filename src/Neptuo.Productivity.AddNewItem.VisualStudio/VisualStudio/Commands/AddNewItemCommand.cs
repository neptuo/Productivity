using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Neptuo.Productivity.VisualStudio.ViewModels;
using Neptuo.Productivity.VisualStudio.Views;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Interop;
using Window = System.Windows.Window;

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

        private async void OnExecute(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            DTE dte = await package.GetServiceAsync<DTE>();
            ITemplateService templateService = await package.GetComponentServiceAsync<FirstNotNullTemplateService>();

            var viewModel = new MainViewModel(new DteFileService(dte, package), templateService, new DteCursorService(dte));
            SetViewModelPath(dte, viewModel);

            var wnd = new AddNewItemWindow(viewModel);
            SetWindowOwner(dte, wnd);

            wnd.ShowDialog();
        }

        private static void SetWindowOwner(DTE dte, AddNewItemWindow wnd)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var ownerHwnd = new IntPtr(dte.MainWindow.HWnd);
            wnd.Owner = (Window)HwndSource.FromHwnd(ownerHwnd).RootVisual;
        }

        private static void SetViewModelPath(DTE dte, MainViewModel viewModel)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (dte.SelectedItems.Count == 1)
            {
                SelectedItem item = dte.SelectedItems.Item(1);
                string path = null;
                if (item.ProjectItem != null)
                    path = item.ProjectItem.FileNames[0];
                else if (item.Project != null)
                    path = Path.GetDirectoryName(item.Project.FileName);
                else if (dte.Solution != null)
                    path = Path.GetDirectoryName(dte.Solution.FileName);

                if (path != null)
                {
                    if (File.Exists(path))
                        path = Path.GetDirectoryName(path);

                    viewModel.Path = path;
                }
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
