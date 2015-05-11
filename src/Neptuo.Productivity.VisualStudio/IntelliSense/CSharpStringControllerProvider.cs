using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Productivity.VisualStudio.IntelliSense.Completions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense
{
    [Export(typeof(IVsTextViewCreationListener))]
    [Name("token completion handler")]
    [ContentType(CSharpCompletionSourceProvider.ContentType)]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal class CSharpStringControllerProvider : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService AdapterService { get; set; }
        [Import]
        internal ICompletionBroker CompletionBroker { get; set; }
        [Import]
        internal SVsServiceProvider ServiceProvider { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            ITextView textView = AdapterService.GetWpfTextView(textViewAdapter);
            if (textView == null)
                return;

            Func<CSharpStringController> createCommandHandler = delegate()
            {
                CSharpStringController controller = new CSharpStringController(textViewAdapter, textView, CompletionBroker, ServiceProvider);
                return controller;
            };
            textView.Properties.GetOrCreateSingletonProperty(createCommandHandler);
        }
    }
}
