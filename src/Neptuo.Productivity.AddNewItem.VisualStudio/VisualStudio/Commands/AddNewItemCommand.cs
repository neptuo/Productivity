﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Neptuo.Collections.Specialized;
using Neptuo.Productivity.VisualStudio.ViewModels;
using Neptuo.Productivity.VisualStudio.Views;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Windows.Interop;
using Window = System.Windows.Window;
using Task = System.Threading.Tasks.Task;

namespace Neptuo.Productivity.VisualStudio.Commands
{
    /// <summary>
    /// Binds 'Add New Item' commands.
    /// </summary>
    internal sealed class AddNewItemCommand
    {
        private readonly VsPackage package;

        private AddNewItemCommand(VsPackage package, IMenuCommandService commandService)
        {
            this.package = package;

            WireUpMenuCommands(commandService);
        }

        private void WireUpMenuCommands(IMenuCommandService commandService)
        {
            CommandID commandId = new CommandID(PackageGuids.CommandSet, PackageIds.AddNewItem);
            MenuCommand command = new MenuCommand(OnExecuteAsync, commandId);
            commandService.AddCommand(command);
        }

        private async void OnExecuteAsync(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            DTE dte = await package.GetServiceAsync<DTE>();
            ITemplateService templates = await package.GetComponentServiceAsync<FirstNotNullTemplateService>();
            IParameterService parameterProvider = await package.GetComponentServiceAsync<ManyParameterService>();
            IFileService files = new DteFileService(dte, package);
            ICursorService cursor = new DteCursorService();

            var viewModel = new MainViewModel(files);
            SetViewModelPath(dte, viewModel);

            var wnd = new AddNewItemWindow(viewModel);
            SetWindowOwner(dte, wnd);

            if (wnd.ShowDialog() != true)
                return;

            await CreateItemAsync(templates, files, parameterProvider, cursor, viewModel);
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

        private static async Task CreateItemAsync(ITemplateService templates, IFileService files, IParameterService parameterProvider, ICursorService cursor, MainViewModel viewModel)
        {
            string path = Path.GetFullPath(Path.Combine(viewModel.Path, viewModel.Name));
            if (viewModel.IsFile)
            {
                ITemplate template = templates.FindTemplate(path) ?? EmptyTemplate.Instance;
                if (template is IContentTemplate contentTemplate)
                {
                    KeyValueCollection parameters = new KeyValueCollection();
                    parameterProvider.Add(path, parameters);

                    TemplateContent templateContent = contentTemplate.GetContent(parameters);

                    files.CreateFile(path, templateContent.Encoding, templateContent.Content);

                    if (templateContent.Position > 0)
                        cursor.Move(path, templateContent.Position);
                }
                else if (template is IApplicableTemplate applicableTemplate)
                {
                    await applicableTemplate.ApplyAsync(path, new KeyValueCollection());
                }
                else
                {
                    throw Ensure.Exception.NotSupported($"We don't know how to apply the template of type '{template.GetType().FullName}'.");
                }
            }
            else
            {
                files.CreateDirectory(path);
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
