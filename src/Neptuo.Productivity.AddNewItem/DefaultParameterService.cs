using Neptuo.Collections.Specialized;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class DefaultParameterService : IParameterService
    {
        public IReadOnlyKeyValueCollection Get(string filePath)
        {
            return new KeyValueCollection();
        }
    }
}
