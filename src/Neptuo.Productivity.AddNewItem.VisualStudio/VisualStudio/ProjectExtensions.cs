using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    internal static class ProjectExtensions
    {
        public static bool IsKind(this Project project, params string[] kindGuids)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            foreach (string guid in kindGuids)
            {
                if (project.Kind.Equals(guid, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public static string FindRootFolder(this Project project)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (project == null)
                return null;

            if (project.IsKind("{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")) // ProjectKinds.vsProjectKindSolutionFolder
                return Path.GetDirectoryName(project.DTE.Solution.FullName);

            if (string.IsNullOrEmpty(project.FullName))
                return null;

            string fullPath;

            try
            {
                fullPath = project.Properties.Item("FullPath").Value as string;
            }
            catch (ArgumentException)
            {
                try
                {
                    // MFC projects don't have FullPath, and there seems to be no way to query existence
                    fullPath = project.Properties.Item("ProjectDirectory").Value as string;
                }
                catch (ArgumentException)
                {
                    // Installer projects have a ProjectPath.
                    fullPath = project.Properties.Item("ProjectPath").Value as string;
                }
            }

            if (string.IsNullOrEmpty(fullPath))
                return File.Exists(project.FullName) ? Path.GetDirectoryName(project.FullName) : null;

            if (Directory.Exists(fullPath))
                return fullPath;

            if (File.Exists(fullPath))
                return Path.GetDirectoryName(fullPath);

            return null;
        }

        public static string FindRootNamespace(this Project project)
        {
            if (project == null)
                return null;

            string result = FindProperty(project, "RootNamespace") ?? project.Name ?? string.Empty;

            return CleanNamespace(result, stripPeriods: false);
        }

        public static string FindProperty(this Project project, string propertName)
        {
            try
            {
                Property property = project.Properties.Item(propertName);
                string propertyValue = property.Value.ToString();

                if (property != null && property.Value != null && !string.IsNullOrEmpty(propertyValue))
                    return propertyValue;
            }
            catch
            {
                /* Project doesn't have a root namespace */
            }

            return null;
        }

        public static string CleanNamespace(string ns, bool stripPeriods = true)
        {
            if (stripPeriods)
                ns = ns.Replace(".", "");

            ns = ns.Replace(" ", "")
                .Replace("-", "")
                .Replace("\\", ".");

            return ns;
        }
    }
}
