using Neptuo.FileSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class FileTemplate : SnippetTemplate
    {
        private readonly string filePath;
        private readonly string content;

        public override Encoding Encoding { get; }

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

        protected override string GetContent() => content;
    }
}
