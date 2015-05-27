using EnvDTE;
using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class BuildCancelServiceActivator : IActivator<BuildCancelService>
    {
        private readonly DTE dte;

        public BuildCancelServiceActivator(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
        }

        public BuildCancelService Create()
        {
            return new BuildCancelService(dte);
        }
    }
}
