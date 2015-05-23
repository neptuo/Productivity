using EnvDTE;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Options
{
    [Export(typeof(IConfiguration))]
    public class DteConfiguration : IConfiguration
    {
        #region Infrastructure

        private readonly DTE dte;

        public DteConfiguration(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
        }

        private Properties GetGeneralProperties()
        {
            Properties properties = dte.get_Properties(MyConstants.Feature.MainCategory, MyConstants.Feature.GeneralPage);
            return properties;
        }

        private T GetGeneralPropertyValue<T>([CallerMemberName] string propertyName = null)
        {
            return GetGeneralProperties().Item(propertyName).Value;
        }

        #endregion

        public bool IsUnderscoreNamespaceRemoverUsed
        {
            get { return GetGeneralPropertyValue<bool>(); }
        }

        public bool IsLineDuplicatorUsed
        {
            get { return GetGeneralPropertyValue<bool>(); }
        }
    }
}
