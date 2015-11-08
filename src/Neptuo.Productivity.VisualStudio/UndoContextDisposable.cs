using EnvDTE;
using Neptuo.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public class UndoContextDisposable : DisposableBase
    {
        private readonly DTE dte;

        public UndoContextDisposable(DTE dte, string name)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
            dte.UndoContext.Open(name);
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            dte.UndoContext.Close();
        }
    }
}
