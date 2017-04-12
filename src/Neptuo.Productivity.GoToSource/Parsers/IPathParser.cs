using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    public interface IPathParser
    {
        bool TryParse(string line, int index, out string path);
    }
}
