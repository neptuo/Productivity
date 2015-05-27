using EnvDTE;
using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Misc
{
    public class StartPageServiceActivator : IActivator<StartPageService>
    {
        private readonly DTE dte;

        public StartPageServiceActivator(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
        }

        public StartPageService Create()
        {
            return new StartPageService(dte);
        }
    }
}
