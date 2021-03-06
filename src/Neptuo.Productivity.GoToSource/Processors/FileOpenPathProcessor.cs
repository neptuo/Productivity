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

namespace Neptuo.Productivity.Processors
{
    [Export(typeof(IPathProcessor))]
    [Name(Name)]
    [Order(After = ProjectItemPathProcessor.Name)]
    public class FileOpenPathProcessor : IPathProcessor
    {
        public const string Name = "Open file Processor";

        public bool TryRun(string path)
        {
            if (File.Exists(path))
            {
                DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
                dte.ExecuteCommand("File.OpenFile", path);
                return true;
            }

            return false;
        }
    }
}
