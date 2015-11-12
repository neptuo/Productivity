using EnvDTE;
using Neptuo.Pipelines.Events;
using Neptuo.Pipelines.Queries;
using Neptuo.PresentationModels;
using Neptuo.PresentationModels.TypeModels;
using Neptuo.Productivity.VisualStudio.Options;
using Neptuo.Productivity.VisualStudio.UI.Options;
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
        public static IModelDefinition ConfigurationDefinition { get; private set; }

        public static VsServiceContainer VsServices { get; private set; }

        public static void Initialize(FeaturePage configuration)
        {
            DefaultEventManager eventManager = new DefaultEventManager();
            EventDispatcher = eventManager;
            EventRegistry = eventManager;

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
