using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class TemplateContent
    {
        public Encoding Encoding { get; }
        public string Content { get; }
        public int Position { get; }

        public TemplateContent(Encoding encoding, string content)
            : this(encoding, content, 0)
        { }

        public TemplateContent(Encoding encoding, string content, int position)
        {
            Encoding = encoding;
            Content = content;
            Position = position;
        }
    }
}
