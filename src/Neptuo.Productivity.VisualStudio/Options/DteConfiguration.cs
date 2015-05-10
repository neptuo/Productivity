using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Options
{
    internal class DteConfiguration : IConfiguration
    {
        private readonly DTE dte;

        public DteConfiguration(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
        }

        public bool IsUnderscoreNamespaceRemoverUsed
        {
            get { throw new NotImplementedException(); }
        }
    }
}
