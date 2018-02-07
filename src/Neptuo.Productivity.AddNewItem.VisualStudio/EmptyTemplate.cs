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

        public TemplateContent GetContent(IReadOnlyKeyValueCollection parameters) => new TemplateContent(Encoding.UTF8, String.Empty);
    }
}
