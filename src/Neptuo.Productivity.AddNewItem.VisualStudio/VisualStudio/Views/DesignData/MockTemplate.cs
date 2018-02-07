using Neptuo.Collections.Specialized;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Views.DesignData
{
    public class MockTemplate : ITemplate
    {
        public TemplateContent GetContent(IReadOnlyKeyValueCollection parameters)
        {
            return new TemplateContent(Encoding.UTF8, String.Empty, 0);
        }
    }
}
