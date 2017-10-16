using Neptuo.Collections.Specialized;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class EmptyTemplate : ITemplate
    {
        public static EmptyTemplate Instance { get; } = new EmptyTemplate();

        public Encoding Encoding => Encoding.UTF8;
        public TemplateContent GetContent(IReadOnlyKeyValueCollection parameters) => new TemplateContent(String.Empty);
    }
}
