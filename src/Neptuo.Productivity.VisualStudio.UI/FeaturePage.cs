using Microsoft.VisualStudio.Shell;
using Neptuo.Pipelines.Events;
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
        private IConfiguration configuration;
        private IEventDispatcher eventDispatcher;

        private bool isUnderscoreNamespaceRemoverUsed;

        [Category(MyConstants.Feature.FriendlyNamespaces)]
        [DisplayName("Use namespace underscore remover.")]
        [Description("Removes parts of the C# namespace that starts with '_'.")]
        public bool IsUnderscoreNamespaceRemoverUsed { get; set; }

        public FeaturePage()
        {
            configuration = ServiceFactory.Configuration;
            eventDispatcher = ServiceFactory.EventDispatcher;
            isUnderscoreNamespaceRemoverUsed = ServiceFactory.Configuration.IsUnderscoreNamespaceRemoverUsed;
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            if (isUnderscoreNamespaceRemoverUsed != IsUnderscoreNamespaceRemoverUsed)
                eventDispatcher.PublishAsync(new IsUnderscoreNamespaceRemoverUsedChangedEvent(IsUnderscoreNamespaceRemoverUsed)).RunSynchronously();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
