using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo;
using Neptuo.Collections.Specialized;
using Neptuo.Productivity.VisualStudio;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    [Export(typeof(IParameterService))]
    public class CSharpParameterService : IParameterService
    {
        private readonly DTE dte;

        public CSharpParameterService() => dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));

        public void Add(string filePath, IKeyValueCollection parameters)
        {
            string fileExtension = Path.GetExtension(filePath);
            if (fileExtension == ".cs")
            {
                Project project = FindSelectedProject();
                if (project != null)
                {
                    string relative = PackageUtilities.MakeRelative(project.FindRootFolder(), Path.GetDirectoryName(filePath));
                    string rootNamespace = project.FindRootNamespace();
                    string ns = ConcatNamespace(rootNamespace, ProjectExtensions.CleanNamespace(relative));

                    parameters.Add("namespace", ns);
                }

                parameters.Add("itemname", Path.GetFileNameWithoutExtension(filePath));
            }
        }

        private Project FindSelectedProject()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (dte.SelectedItems.Count == 1)
            {
                SelectedItem item = dte.SelectedItems.Item(1);
                return item.ProjectItem.ContainingProject;
            }

            return null;
        }

        private string ConcatNamespace(string ns1, string ns2)
        {
            if (!String.IsNullOrEmpty(ns1))
                return ns2;

            if (!String.IsNullOrEmpty(ns2))
                return ns1;

            return ns1 + "." + ns2;
        }
    }
}
