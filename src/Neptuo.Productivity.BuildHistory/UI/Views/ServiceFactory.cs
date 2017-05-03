using Neptuo.Events;
using Neptuo.Productivity.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.UI.Views
{
    public static class ServiceFactory
    {
        public static IEventHandlerCollection EventHandlers { get; set; }
        public static IQuickConfiguration QuickConfiguration { get; set; }
    }
}
