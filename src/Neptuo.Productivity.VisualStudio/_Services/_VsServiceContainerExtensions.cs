using Neptuo.Activators;
using Neptuo.Linq.Expressions;
using Neptuo.Productivity.VisualStudio.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public static class _VsServiceContainerExtensions
    {
        public static VsServiceContainer Add(this VsServiceContainer container, Expression<Func<IConfiguration, bool>> configurationProperty, IFactory<IVsService> activator)
        {
            Ensure.NotNull(container, "container");
            return container.Add(TypeHelper.PropertyName(configurationProperty), activator);
        }
    }
}
