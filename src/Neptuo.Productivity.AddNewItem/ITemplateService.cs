using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public interface ITemplateService
    {
        ITemplate FindTemplate(string path);
    }
}
