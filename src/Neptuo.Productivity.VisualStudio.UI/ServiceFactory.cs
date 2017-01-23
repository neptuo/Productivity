using EnvDTE;
using Neptuo.Events;
using Neptuo.Queries;
using Neptuo.PresentationModels;
using Neptuo.PresentationModels.TypeModels;
using Neptuo.Productivity.VisualStudio.Options;
using Neptuo.Productivity.VisualStudio.UI.Options;
using Neptuo.Queries;
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
        
        public static IEventHandlerCollection EventHandlers { get; private set; }
        public static IEventDispatcher EventDispatcher { get; private set; }

        public static IConfiguration Configuration { get; private set; }
        public static IModelDefinition ConfigurationDefinition { get; private set; }

        public static VsServiceContainer VsServices { get; private set; }

        public static void Initialize(FeaturePage configuration)
        {
            DefaultEventManager eventManager = new DefaultEventManager();
            EventDispatcher = eventManager;
            EventHandlers = eventManager;

            Configuration = configuration;
            ConfigurationDefinition = new ReflectionModelDefinitionBuilder(Configuration.GetType(), new AttributeMetadataReaderCollection()).Create();
            VsServices = new VsServiceContainer();
        }

        public static void Dispose()
        {
            if (VsServices != null)
                VsServices.Disponse();
        }
    }
}
