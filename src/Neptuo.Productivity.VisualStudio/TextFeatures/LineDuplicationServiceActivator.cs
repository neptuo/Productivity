using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.TextFeatures
{
    public class LineDuplicationServiceActivator : IFactory<LineDuplicationService>
    {
        private readonly DTE dte;
        private readonly OleMenuCommandService commandService;

        public LineDuplicationServiceActivator(DTE dte, OleMenuCommandService commandService)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(commandService, "commandService");
            this.dte = dte;
            this.commandService = commandService;
        }

        public LineDuplicationService Create()
        {
            return new LineDuplicationService(dte, commandService);
        }
    }
}
