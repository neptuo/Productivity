using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public interface IParameterService
    {
        void Add(string filePath, IKeyValueCollection parameters);
    }
}
