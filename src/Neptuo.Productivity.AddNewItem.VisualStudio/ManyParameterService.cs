using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Collections.Specialized;

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
