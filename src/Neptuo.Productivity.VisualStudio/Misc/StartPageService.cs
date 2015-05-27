using EnvDTE;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Misc
{
    public class StartPageService : DisposableBase, IVsService
    {
        private readonly DTE dte;
        private readonly SolutionEvents events;

        public StartPageService(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
            this.events = dte.Events.SolutionEvents;

            events.AfterClosing += OnAfterClosing;
        }

        private void OnAfterClosing()
        {
            dte.ExecuteCommand("View.StartPage");
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            events.AfterClosing -= OnAfterClosing;
        }
    }
}
