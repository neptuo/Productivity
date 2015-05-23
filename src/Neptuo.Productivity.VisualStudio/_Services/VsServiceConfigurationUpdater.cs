using Neptuo.PresentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public class VsServiceConfigurationUpdater
    {
        private readonly VsServiceContainer vsServices;
        private readonly DictionaryModelValueProvider configurationSnapshot;
        private readonly CopyModelValueProvider copyProvider;

        public VsServiceConfigurationUpdater(VsServiceContainer vsServices, IModelDefinition modelDefinition, IModelValueGetter getter)
        {
            this.vsServices = vsServices;
            configurationSnapshot = new DictionaryModelValueProvider();

            copyProvider = new CopyModelValueProvider(modelDefinition, false);
            copyProvider.Update(configurationSnapshot, getter);
        }

        public void Update(IModelValueGetter getter)
        {
            List<string> toRun = new List<string>();
            List<string> toStop = new List<string>();

            Dictionary<string, IFieldDefinition> fields = copyProvider.ModelDefinition.FieldsByIdentifier();
            ObservableModelValueProvider observableConfiguration = new ObservableModelValueProvider(configurationSnapshot);
            observableConfiguration.PropertyChanged += (sender, pe) =>
            {
                IFieldDefinition field;
                if (fields.TryGetValue(pe.PropertyName, out field))
                {
                    if (field.FieldType == typeof(bool))
                    {
                        bool value = getter.GetValueOrDefault(pe.PropertyName, false);
                        if (value)
                            toRun.Add(pe.PropertyName);
                        else
                            toStop.Add(pe.PropertyName);
                    }
                }
            };
            copyProvider.Update(observableConfiguration, getter);

            if (toRun.Any())
                vsServices.RunServices(toRun);

            if (toStop.Any())
                vsServices.StopServices(toStop);
        }
    }
}
