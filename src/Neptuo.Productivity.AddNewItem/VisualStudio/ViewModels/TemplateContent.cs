using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.ViewModels
{
    public class TemplateContent
    {
        public string Content { get; private set; }
        public int Position { get; private set; }

        public TemplateContent(string content)
            : this(content, 0)
        { }

        public TemplateContent(string content, int position)
        {
            Content = content;
            Position = position;
        }
    }
}
