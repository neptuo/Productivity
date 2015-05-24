using Microsoft.VisualStudio.Shell;
using Neptuo.Pipelines.Events;
using Neptuo.PresentationModels;
using Neptuo.PresentationModels.TypeModels;
using Neptuo.Productivity.VisualStudio.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.UI
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class FeaturePage : DialogPage, IConfiguration
    {
        private ReflectionModelValueProvider thisProvider;
        private VsServiceConfigurationUpdater updater;

        [Category(MyConstants.Feature.FriendlyNamespaces)]
        [DisplayName("Use namespace underscore remover.")]
        [Description("Removes parts of the C# namespace that starts with '_', e.g. 'Tests._Models.Domain' -> 'Tests.Domain'.")]
        public bool IsUnderscoreNamespaceRemoverUsed { get; set; }

        [Category(MyConstants.Feature.LineDuplications)]
        [DisplayName("Allow current line duplication using keyboard.")]
        [Description("Allows to duplicate of current line over or under the current line.")]
        public bool IsLineDuplicatorUsed { get; set; }

        [Category(MyConstants.Feature.Builds)]
        [DisplayName("Store build history for the current VisualStudio instance.")]
        [Description("Allows to show window with build history in the current VisualStudio instance.")]
        public bool IsBuildHistoryUsed { get; set; }

        [Category(MyConstants.Feature.Builds)]
        [DisplayName("Automatically arrange whether project should or should not be build.")]
        [Description("...")]
        public bool IsBuildAutoConfigurationUsed { get; set; }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            thisProvider = new ReflectionModelValueProvider(this);
            updater = new VsServiceConfigurationUpdater(ServiceFactory.VsServices, ServiceFactory.ConfigurationDefinition, thisProvider);
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);

            if (e.ApplyBehavior == ApplyKind.Apply)
                updater.Update(thisProvider);
        }
    }
}
