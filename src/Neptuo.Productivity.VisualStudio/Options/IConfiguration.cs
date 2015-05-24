using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Options
{
    public interface IConfiguration
    {
        bool IsUnderscoreNamespaceRemoverUsed { get; }

        bool IsLineDuplicatorUsed { get; }

        bool IsBuildHistoryUsed { get; }

        bool IsBuildAutoConfigurationUsed { get; }
    }
}
