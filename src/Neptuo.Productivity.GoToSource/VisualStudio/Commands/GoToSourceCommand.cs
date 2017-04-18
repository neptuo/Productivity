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
            CommandID commandId = new CommandID(PackageGuids.CommandSet, PackageIds.GoToSource);
            MenuCommand command = new MenuCommand(OnExecute, commandId);
            commandService.AddCommand(command);
        }

        private void OnExecute(object sender, EventArgs e)
        {
            OnExecute();
        }

        private bool OnExecute()
        {
            if (dte.ActiveDocument != null)
            {
                TextDocument textDocument = dte.ActiveDocument.GetTextDocument();
                if (textDocument != null)
                {
                    GoToSourceService service = GetService<GoToSourceService>();
                    return service.TryRun(textDocument);
                }
            }

            return false;
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

        public static bool TryExecute()
        {
            if (instance != null)
                return instance.OnExecute();

            return false;
        }

        #endregion
    }
}
