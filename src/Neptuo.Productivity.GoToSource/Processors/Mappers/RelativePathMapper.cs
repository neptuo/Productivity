using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Processors.Mappers
{
    [Export(typeof(IPathMapper))]
    [Name(MapperName.Relative)]
    [Order(After = MapperName.Virtual)]
    public class RelativePathMapper : IPathMapper
    {
        public string Map(string source)
        {
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            if (!String.IsNullOrEmpty(dte.ActiveWindow.Document.FullName))
            {
                if (!Path.IsPathRooted(source) && !source.StartsWith("~/"))
                    source = Path.Combine(Path.GetDirectoryName(dte.ActiveWindow.Document.FullName), source);
            }

            return source;
        }
    }
}
