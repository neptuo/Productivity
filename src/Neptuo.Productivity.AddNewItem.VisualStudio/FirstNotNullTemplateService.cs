using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    [Export(typeof(FirstNotNullTemplateService))]
    public class FirstNotNullTemplateService : ITemplateService
    {
        private bool isOrdered;

        [ImportMany]
        public IEnumerable<Lazy<ITemplateService, IOrderable>> Services { get; set; }

        public ITemplate FindTemplate(string path)
        {
            if (!isOrdered)
            {
                Services = Orderer.Order(Services);
                isOrdered = true;
            }

            foreach (Lazy<ITemplateService> service in Services)
            {
                ITemplate template = service.Value.FindTemplate(path);
                if (template != null)
                    return template;
            }

            return null;
        }
    }
}
