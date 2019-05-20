using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            T service = (T)serviceProvider.GetService(typeof(T));
            if (service == null)
                throw Ensure.Exception.InvalidOperation($"Unnable to get '{typeof(T).FullName}' service.");

            return service;
        }

        public static TContract GetService<TService, TContract>(this IServiceProvider serviceProvider)
            where TContract : class
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            TContract service = (TContract)serviceProvider.GetService(typeof(TService));
            if (service == null)
                throw Ensure.Exception.InvalidOperation($"Unnable to get '{typeof(TService).FullName}' service.");

            return service;
        }
    }
}
