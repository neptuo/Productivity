using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Views.DesignData
{
    internal class MockTemplateService : ITemplateService
    {
        public ITemplate FindTemplate(string path)
        {
            return new MockTemplate();
        }
    }
}
