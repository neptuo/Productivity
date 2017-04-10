using Microsoft.VisualStudio.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TextManager.Interop;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Shell;

namespace Neptuo.Productivity.VisualStudio.Listeners
{
    /// <summary>
    /// Text view creation listener for attaching F12 (GoToDefinition) commands.
    /// </summary>
    [Export(typeof(IVsTextViewCreationListener))]
    [Name("GoToSource Command Listener.")]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal class VsTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService { get; set; }

        [Import]
        internal SVsServiceProvider ServiceProvider { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            textView.Properties.GetOrCreateSingletonProperty(() => new VsCommandFilter(textViewAdapter, ServiceProvider));
        }
    }
}
