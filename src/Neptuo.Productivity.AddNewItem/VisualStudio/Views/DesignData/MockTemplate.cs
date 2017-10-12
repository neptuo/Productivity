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
        public Encoding Encoding => Encoding.UTF8;

        public TemplateContent GetContent(IReadOnlyKeyValueCollection parameters)
        {
            return new TemplateContent(String.Empty, 0);
        }
    }
}
