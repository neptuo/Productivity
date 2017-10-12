using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.ViewModels
{
    public interface IFileService
    {
        bool FileExists(string path);
        bool DirectoryExists(string path);

        bool IsValidFileName(string name);
        bool IsValidDirectoryName(string name);

        void CreateFile(string path, ITemplate template);
        void CreateDirectory(string path);
    }
}
