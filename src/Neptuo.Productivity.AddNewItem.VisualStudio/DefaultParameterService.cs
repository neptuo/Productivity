using Neptuo.Collections.Specialized;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    [Export(typeof(IParameterService))]
    public class DefaultParameterService : IParameterService
    {
        public void Add(string filePath, IKeyValueCollection parameters)
        { }
    }
}
