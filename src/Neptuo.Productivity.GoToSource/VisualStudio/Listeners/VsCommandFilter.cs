using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Listeners
{
    internal class VsCommandFilter : IOleCommandTarget
    {
        private readonly IOleCommandTarget nextController;
        private readonly SVsServiceProvider serviceProvider;

        public VsCommandFilter(IVsTextView textViewAdapter, SVsServiceProvider serviceProvider)
        {
            textViewAdapter.AddCommandFilter(this, out nextController);
            this.serviceProvider = serviceProvider;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return nextController.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (nCmdID == (uint)VSConstants.VSStd97CmdID.GotoDefn)
            {
                Console.WriteLine("Go to...");
            }
            
            int nextResult = nextController.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
            return nextResult;
        }
    }
}
