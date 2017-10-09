using EnvDTE;
using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public class FindDialogListener : DisposableBase
    {
        private readonly DTE dte;
        private readonly CommandEvents events;

        public FindDialogListener(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;

            events = dte.Events.CommandEvents;
            events.AfterExecute += OnCommandExecuted;
        }

        private void OnCommandExecuted(string Guid, int ID, object CustomIn, object CustomOut)
        {

        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            events.AfterExecute -= OnCommandExecuted;
        }
    }
}
