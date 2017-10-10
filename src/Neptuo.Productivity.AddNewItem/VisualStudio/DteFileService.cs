using EnvDTE;
using Neptuo;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public class DteFileService : IFileService
    {
        private readonly DTE dte;

        public DteFileService(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
        }

        private bool TryGetSolutionPath(out string solutionPath)
        {
            if (dte.Solution == null)
            {
                solutionPath = null;
                return false;
            }

            solutionPath = Path.GetDirectoryName(dte.Solution.FileName);
            return true;
        }

        public bool DirectoryExists(string path)
        {
            if (TryGetSolutionPath(out string solutionPath))
                return Directory.Exists(Path.Combine(solutionPath, path));

            return true;
        }

        public bool FileExists(string path)
        {
            if (TryGetSolutionPath(out string solutionPath))
                return File.Exists(Path.Combine(solutionPath, path));

            return true;
        }

        public bool IsValidFileName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            if (name.Contains(Path.DirectorySeparatorChar) || name.Contains(Path.AltDirectorySeparatorChar))
            {
                string path = Path.GetDirectoryName(name);
                if (!IsValidDirectoryName(path))
                    return false;

                name = Path.GetFileName(name);
            }

            foreach (char item in Path.GetInvalidFileNameChars())
            {
                if (name.Contains(item))
                    return false;
            }

            return true;
        }

        public bool IsValidDirectoryName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            foreach (char item in Path.GetInvalidPathChars())
            {
                if (name.Contains(item))
                    return false;
            }

            return true;
        }
    }
}
