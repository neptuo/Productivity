using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    public interface IPathReader
    {
        bool TryRead(TextDocument textDocument, out string path);
    }
}
