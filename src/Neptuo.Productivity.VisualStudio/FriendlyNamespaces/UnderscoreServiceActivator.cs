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
    public class UnderscoreServiceActivator : IActivator<UnderscoreService>
    {
        public UnderscoreService Create()
        {
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            Ensure.NotNull(dte, "dte");

            ProjectItemsEvents csharpProjectItemsEvents = (ProjectItemsEvents)dte.Events.GetObject("CSharpProjectItemsEvents");
            Ensure.NotNull(csharpProjectItemsEvents, "csharpProjectItemsEvents");

            OleMenuCommandService commandService = (OleMenuCommandService)ServiceProvider.GlobalProvider.GetService(typeof(IMenuCommandService));
            Ensure.NotNull(commandService, "commandService");

            return new UnderscoreService(dte, commandService, csharpProjectItemsEvents);
        }
    }
}
