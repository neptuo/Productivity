using EnvDTE;
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
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Editor;
using Neptuo;
using Microsoft.VisualStudio;

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
            MenuCommand command = new MenuCommand(OnExecuteAsync, commandId);
            commandService.AddCommand(command);
        }

        private async void OnExecuteAsync(object sender, EventArgs e)
        {
            DTE dte = await package.GetServiceAsync<DTE>();

            //string commandName = "Edit.InvokeSnippetFromShortcut";
            //string commandArgs = "class";
            //string commandName = "Edit.InsertSnippet";
            //string commandArgs = "";

            try
            {
                dte.ActiveDocument.Activate();

                //dte.ExecuteCommand(commandName, commandArgs);

                ((TextSelection)dte.ActiveDocument.Selection).Text = "class";
                dte.ExecuteCommand("Edit.InsertTab");
                dte.ExecuteCommand("Edit.InsertTab");

                //dte.ExecuteCommand("Edit.InvokeSnippetFromShortcut");

                //((TextSelection)dte.ActiveDocument.Selection).Text = "Ensure.NotNull($name$, \"$name$\");$end$";
                //dte.ExecuteCommand("Edit.ShowSnippetHighlighting");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }


            try
            {
                DTE dte = await package.GetServiceAsync<DTE>();

                IVsTextManager textManager = await package.GetServiceAsync<SVsTextManager, IVsTextManager>();
                IVsTextManager2 textManager2 = (IVsTextManager2)textManager;

                IVsTextView textView;
                textManager.GetActiveView(1, null, out textView);

                IVsEditorAdaptersFactoryService adapterFactory = await package.GetComponentServiceAsync<IVsEditorAdaptersFactoryService>();
                IWpfTextView wpfTextView = adapterFactory.GetWpfTextView(textView);

                IVsTextBuffer textBuffer = adapterFactory.GetBufferAdapter(wpfTextView.TextBuffer);
                IVsExpansion expansion = (IVsExpansion)textBuffer;

                textView.GetCaretPos(out int line, out int column);

                var textSpan = new TextSpan()
                {
                    iStartLine = line,
                    iStartIndex = column,
                    iEndLine = line,
                    iEndIndex = column
                };

                //https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.textmanager.interop.ivsexpansion?view=visualstudiosdk-2017
                //https://github.com/dotnet/roslyn/blob/defeca5d0e44915fc978cfd93d3de2e4a6d0dbf4/src/VisualStudio/Core/Def/Implementation/Snippets/AbstractSnippetExpansionClient.cs
                //https://github.com/adamdriscoll/poshtools/blob/ad99bb6ff6a284c39bf54611cc40a42b603f4277/PowerShellTools/Snippets/SnippetHandler.cs

                IVsExpansionSession session;
                Guid languageGuid = Guid.Parse("694dd9b6-b865-4c5b-ad85-86356e9c88dc");
                //string title = "Ensure.NotNull";
                //string path = @"D:\Development\Neptuo\Productivity\src\Neptuo.Productivity.CodeSnippets\CodeSnippets\CSharp\Neptuo\Neptuo.Ensure-NotNull.snippet";
                string title = "attribute";
                string path = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC#\Snippets\1033\Visual C#\attribute.snippet";
                //expansion.InsertExpansion(textSpan, textSpan, null, languageGuid, out session);
                //expansion.InsertSpecificExpansion(,,,,);
                expansion.InsertNamedExpansion(title, path, textSpan, new VsExpansionClient(expansion, languageGuid, textSpan), languageGuid, 0, out session);


                //IVsExpansionManager expansionManager;
                //textManager2.GetExpansionManager(out expansionManager);
                //expansionManager.InvokeInsertionUI(textView, new VsExpansionClient(expansion, languageGuid, textSpan), languageGuid, new string[] { "Expansion" }, 1, 0, null, 0, 0, "Expand: ", ".");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }

            //await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            //DTE dte = await package.GetServiceAsync<DTE>();
            //ITemplateService templates = await package.GetComponentServiceAsync<FirstNotNullTemplateService>();
            //IFileService files = new DteFileService(dte, package);
            //ICursorService cursor = new DteCursorService(dte);

            //var viewModel = new MainViewModel(files);
            //SetViewModelPath(dte, viewModel);

            //var wnd = new AddNewItemWindow(viewModel);
            //SetWindowOwner(dte, wnd);

            //if (wnd.ShowDialog() != true)
            //    return;

            //await CreateItemAsync(templates, files, cursor, viewModel);
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

        private static async Task CreateItemAsync(ITemplateService templates, IFileService files, ICursorService cursor, MainViewModel viewModel)
        {
            string path = Path.Combine(viewModel.Path, viewModel.Name);
            if (viewModel.IsFile)
            {
                ITemplate template = templates.FindTemplate(path) ?? EmptyTemplate.Instance;
                if (template is IContentTemplate contentTemplate)
                {
                    // TODO: Parameters.
                    TemplateContent templateContent = contentTemplate.GetContent(new KeyValueCollection());

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
