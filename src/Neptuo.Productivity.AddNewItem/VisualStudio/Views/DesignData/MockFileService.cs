using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Views.DesignData
{
    internal class MockFileService : IFileService
    {
        public void CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void CreateFile(string path, ITemplate template)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            return false;
        }

        public bool FileExists(string path)
        {
            return false;
        }

        public bool IsValidDirectoryName(string name)
        {
            return true;
        }

        public bool IsValidFileName(string name)
        {
            return true;
        }
    }
}
