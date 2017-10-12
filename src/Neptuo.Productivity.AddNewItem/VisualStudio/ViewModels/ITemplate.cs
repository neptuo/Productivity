using Neptuo.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.ViewModels
{
    public interface ITemplate
    {
        Encoding Encoding { get; }

        TemplateContent GetContent(IReadOnlyKeyValueCollection parameters);
    }
}
