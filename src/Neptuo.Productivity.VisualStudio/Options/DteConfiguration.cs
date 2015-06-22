using EnvDTE;
using Neptuo.Productivity.VisualStudio.Builds;
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

        private T GetGeneralPropertyValue<T>(T defaultValue = default(T), [CallerMemberName] string propertyName = null)
        {
            Properties properties = GetGeneralProperties();
            IEnumerable<string> names = properties.OfType<Property>().Select(p => p.Name);
            foreach (Property property in properties)
            {
                if (property.Name == propertyName)
                    return property.Value;
            }

            return default(T);
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

        public bool IsBuildHistoryUsed
        {
            get { return GetGeneralPropertyValue<bool>(); }
        }

        public int BuildHistoryOverviewCount
        {
            get { return GetGeneralPropertyValue<int>(); }
        }

        public bool IsBuildCancelOnFirstErrorUsed
        {
            get { return GetGeneralPropertyValue<bool>(); }
        }

        public BuildCancelWindow OpenWindowAfterBuildCancel
        {
            get { return GetGeneralPropertyValue<BuildCancelWindow>(BuildCancelWindow.None); }
        }

        public bool IsOpenStartPageOnSolutionCloseUsed
        {
            get { return GetGeneralPropertyValue<bool>(); }
        }
    }
}
