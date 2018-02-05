using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.ViewModels
{
    public interface IParameterService
    {
        IReadOnlyKeyValueCollection Get(string filePath);
    }
}
