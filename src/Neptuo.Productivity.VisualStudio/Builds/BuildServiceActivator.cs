using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class BuildServiceActivator : IActivator<BuildService>
    {
        private readonly DTE dte;
        private readonly OleMenuCommandService commandService;
        private readonly EventHandler menuHandler;

        public BuildServiceActivator(DTE dte, OleMenuCommandService commandService, EventHandler menuHandler)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(commandService, "commandService");
            Ensure.NotNull(menuHandler, "menuHandler");
            this.dte = dte;
            this.commandService = commandService;
            this.menuHandler = menuHandler;
        }

        public BuildService Create()
        {
            return new BuildService(dte, commandService, menuHandler);
        }
    }
}
