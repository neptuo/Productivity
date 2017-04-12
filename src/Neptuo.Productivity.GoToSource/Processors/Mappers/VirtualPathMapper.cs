using EnvDTE;
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
    public class VirtualPathMapper : IPathMapper
    {
        [Import]
        public DTE Dte { get; set; }

        public string Map(string source)
        {
            string projectPath = Path.GetDirectoryName(Dte.ActiveWindow.Project.FullName);
            if (source.StartsWith("~/"))
                source = source.Replace("~/", projectPath + @"\");

            return source;
        }
    }
}
