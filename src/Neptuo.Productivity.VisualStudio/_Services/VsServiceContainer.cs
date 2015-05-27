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

        public bool TryGetService<T>(out T service)
        {
            foreach (List<VsServiceContext> contexts in storage.Values)
            {
                foreach (VsServiceContext context in contexts)
                {
                    if(context.ServiceType == typeof(T) && context.IsRunning)
                    {
                        service = (T)context.Instance;
                        return true;
                    }
                }
            }

            service = default(T);
            return false;
        }

        private void ExecuteServices(IEnumerable<string> properties, Action<VsServiceContext> action)
        {
            foreach (string property in properties)
            {
                List<VsServiceContext> services;
                if (storage.TryGetValue(property, out services))
                {
                    foreach (VsServiceContext context in services)
                        action(context);
                }
            }
        }

        public void RunServices(IEnumerable<string> properties)
        {
            ExecuteServices(properties, context =>
            {
                if (!context.IsRunning)
                    context.Instance = context.Activator.Create();
            });
        }

        public void StopServices(IEnumerable<string> properties)
        {
            ExecuteServices(properties, context =>
            {
                if (context.IsRunning)
                {
                    context.Instance.Dispose();
                    context.Instance = null;
                }
            });
        }

        public void Disponse()
        {
            foreach (KeyValuePair<string, List<VsServiceContext>> item in storage)
            {
                foreach (VsServiceContext context in item.Value)
                {
                    if (context.IsRunning)
                        context.Instance.Dispose();
                }
            }
        }

        private class VsServiceContext 
        {
            public IActivator<IVsService> Activator { get; private set; }
            public bool IsRunning { get { return Instance != null; } }
            public IVsService Instance { get; set; }
            public Type ServiceType { get; private set; }

            public VsServiceContext(IActivator<IVsService> activator)
            {
                Ensure.NotNull(activator, "activator");
                Activator = activator;

                foreach (Type interfaceType in activator.GetType().GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IActivator<>))
                        ServiceType = interfaceType.GetGenericArguments().First();
                }
            }
        }
    }
}
