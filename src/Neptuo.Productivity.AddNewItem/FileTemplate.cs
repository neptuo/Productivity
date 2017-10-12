using Neptuo;
using Neptuo.Collections.Specialized;
using Neptuo.FileSystems;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class FileTemplate : ITemplate
    {
        private readonly string filePath;
        private readonly string content;

        public Encoding Encoding { get; }

        public FileTemplate(string filePath)
        {
            Ensure.Condition.FileExists(filePath, "filePath");
            this.filePath = filePath;

            using (StreamReader reader = new StreamReader(filePath))
            {
                content = reader.ReadToEnd();
                Encoding = reader.CurrentEncoding;
            }
        }

        public TemplateContent GetContent(IReadOnlyKeyValueCollection parameters)
        {
            return new TemplateContent(content);
        }
    }
}
