using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class StringTemplate : SnippetTemplate
    {
        private string content;

        public StringTemplate(string content)
        {
            Ensure.NotNullOrEmpty(content, "content");
            this.content = content;
        }

        public override Encoding Encoding => Encoding.UTF8;

        protected override string GetContent() => content;
    }
}