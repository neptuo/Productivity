using EnvDTE;
using Neptuo.Pipelines.Events;
using Neptuo.Pipelines.Queries;
using Neptuo.Productivity.VisualStudio.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.UI
{
    public static class ServiceFactory
    {
        public static IQueryDispatcher QueryDispatcher { get; private set; }
        
        public static IEventRegistry EventRegistry { get; private set; }
        public static IEventDispatcher EventDispatcher { get; private set; }

        public static IConfiguration Configuration { get; private set; }

        public static void Initialize(DTE dte)
        {
            DefaultEventManager eventManager = new DefaultEventManager();
            EventDispatcher = eventManager;
            EventDispatcher = eventManager;

            Configuration = new DteConfiguration(dte);
        }
    }
}
