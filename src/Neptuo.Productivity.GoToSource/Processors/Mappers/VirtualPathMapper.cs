﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
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
    [Name(MapperName.Virtual)]
    public class VirtualPathMapper : IPathMapper
    {
        public string Map(string source)
        {
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            if (!String.IsNullOrEmpty(dte.ActiveWindow.Project.FullName))
            {
                string projectPath = Path.GetDirectoryName(dte.ActiveWindow.Project.FullName);
                if (source.StartsWith("~/"))
                    source = source.Replace("~/", projectPath + @"\");
            }

            return source;
        }
    }
}
