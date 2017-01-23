using EnvDTE;
using Neptuo.Activators;
using Neptuo.Productivity.VisualStudio.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class BuildCancelServiceActivator : IFactory<BuildCancelService>
    {
        private readonly DTE dte;
        private readonly IConfiguration configuration;

        public BuildCancelServiceActivator(DTE dte, IConfiguration configuration)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(configuration, "configuration");
            this.dte = dte;
            this.configuration = configuration;
        }

        public BuildCancelService Create()
        {
            return new BuildCancelService(dte, configuration);
        }
    }
}
