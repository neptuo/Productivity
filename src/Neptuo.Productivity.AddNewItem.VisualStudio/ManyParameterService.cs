using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    [Export(typeof(ManyParameterService))]
    public class ManyParameterService : IParameterService
    {
        [ImportMany]
        public IEnumerable<IParameterService> Services { get; set; }

        public void Add(string filePath, IKeyValueCollection parameters)
        {
            foreach (var item in Services)
                item.Add(filePath, parameters);
        }
    }
}
