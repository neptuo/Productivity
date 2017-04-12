using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Processors
{
    public interface IPathProcessor
    {
        bool TryRun(string path);
    }
}
