using Neptuo.Activators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public class VsServiceContainer
    {
        private readonly Dictionary<string, List<VsServiceContext>> storage = new Dictionary<string, List<VsServiceContext>>();

        public VsServiceContainer Add(string configurationProperty, IActivator<IVsService> activator) 
        {
            Ensure.NotNullOrEmpty(configurationProperty, "configurationProperty");
            Ensure.NotNull(activator, "activator");

            List<VsServiceContext> services;
            if (!storage.TryGetValue(configurationProperty, out services))
                storage[configurationProperty] = services = new List<VsServiceContext>();

            services.Add(new VsServiceContext(activator));
            return this;
        }

        public void ConfigurationChanged(IEnumerable<string> properties)
        {
            foreach (string property in properties)
            {
                List<VsServiceContext> services;
                if (storage.TryGetValue(property, out services))
                {
                    foreach (VsServiceContext context in services)
                    {
                        if (!context.IsRunning)
                            context.Instance = context.Activator.Create();
                    }
                }
            }
        }

        private class VsServiceContext 
        {
            public IActivator<IVsService> Activator { get; private set; }
            public bool IsRunning { get { return Instance != null; } }
            public IVsService Instance { get; set; }

            public VsServiceContext(IActivator<IVsService> activator)
            {
                Ensure.NotNull(activator, "activator");
                Activator = activator;
            }
        }
    }
}
