using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neptuo.Collections.Specialized;

namespace Neptuo.Productivity.VisualStudio.Views.DesignData
{
    public class MockTemplate : ITemplate
    {
        public (string content, int position, Encoding encoding) GetContent(IReadOnlyKeyValueCollection parameters)
        {
            return (string.Empty, 0, Encoding.UTF8);
        }
    }
}
