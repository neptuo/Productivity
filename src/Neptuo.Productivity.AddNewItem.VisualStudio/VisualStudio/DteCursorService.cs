using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public class DteCursorService : ICursorService
    {
        public void Move(string filePath, int position)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IWpfTextView view = FindCurentTextView();
            if (view != null)
                view.Caret.MoveTo(new SnapshotPoint(view.TextBuffer.CurrentSnapshot, position));
        }

        private static IWpfTextView FindCurentTextView()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var componentModel = GetComponentModel();
            if (componentModel == null)
                return null;

            var editorAdapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();
            return editorAdapter.GetWpfTextView(GetCurrentNativeTextView());
        }

        private static IVsTextView GetCurrentNativeTextView()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var textManager = ServiceProvider.GlobalProvider.GetService<SVsTextManager, IVsTextManager>();

            ErrorHandler.ThrowOnFailure(textManager.GetActiveView(1, null, out IVsTextView activeView));
            return activeView;
        }

        public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
    }
}
