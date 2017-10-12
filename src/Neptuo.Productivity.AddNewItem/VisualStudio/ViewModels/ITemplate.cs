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
        (string content, int position, Encoding encoding) GetContent(IReadOnlyKeyValueCollection parameters);
    }
}
