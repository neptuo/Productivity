using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.ViewModels
{
    public interface ITemplateService
    {
        ITemplate GetTemplate(string path);
    }
}
