using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace Neptuo.Productivity.VisualStudio.Listeners
{
    /// <summary>
    /// Text view creation listener for attaching F12 (GoToDefinition) commands.
    /// </summary>
    [Export(typeof(IVsTextViewCreationListener))]
    [Name("GoToSource Command Listener.")]
    [ContentType("code")]
    [ContentType("JSON")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal class VsCommandFilterFactory : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            textView.Properties.GetOrCreateSingletonProperty(() => new VsCommandFilter(textViewAdapter));
        }
    }
}
