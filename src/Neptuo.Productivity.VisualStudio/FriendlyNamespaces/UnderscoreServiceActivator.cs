using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.FriendlyNamespaces
{
    public class UnderscoreServiceActivator : IFactory<UnderscoreService>
    {
        private readonly DTE dte;
        private readonly OleMenuCommandService commandService;

        public UnderscoreServiceActivator(DTE dte, OleMenuCommandService commandService)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(commandService, "commandService");
            this.dte = dte;
            this.commandService = commandService;
        }

        public UnderscoreService Create()
        {
            ProjectItemsEvents csharpProjectItemsEvents = (ProjectItemsEvents)dte.Events.GetObject("CSharpProjectItemsEvents");
            Ensure.NotNull(csharpProjectItemsEvents, "csharpProjectItemsEvents");
            return new UnderscoreService(dte, commandService, csharpProjectItemsEvents);
        }
    }
}
