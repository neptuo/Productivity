using EnvDTE;
using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class AutoConfigurationServiceActivator : IActivator<AutoConfigurationService>
    {
        private readonly DTE dte;
        private readonly VsServiceContainer vsServices;

        public AutoConfigurationServiceActivator(DTE dte, VsServiceContainer vsServices)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(vsServices, "vsServices");
            this.dte = dte;
            this.vsServices = vsServices;
        }

        public AutoConfigurationService Create()
        {
            return new AutoConfigurationService(dte, vsServices);
        }
    }
}
