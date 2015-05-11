using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Productivity.VisualStudio.IntelliSense;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Neptuo.Productivity.VisualStudio.IntelliSense
{
    internal class CSharpStringController : IOleCommandTarget
    {
        private readonly IOleCommandTarget nextController;
        private readonly ITextView textView;
        private readonly SVsServiceProvider serviceProvider;
        private readonly CompletionSession completionSession;

        internal CSharpStringController(IVsTextView textViewAdapter, ITextView textView, ICompletionBroker completionBroker, SVsServiceProvider serviceProvider)
        {
            this.textView = textView;
            this.serviceProvider = serviceProvider;
            this.completionSession = new CompletionSession(textView, completionBroker);

            //add the command to the command chain
            textViewAdapter.AddCommandFilter(this, out nextController);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup == VSConstants.VSStd2K)
            {
                switch ((VSConstants.VSStd2KCmdID)prgCmds[0].cmdID)
                {
                    case VSConstants.VSStd2KCmdID.AUTOCOMPLETE:
                    case VSConstants.VSStd2KCmdID.SHOWMEMBERLIST:
                    case VSConstants.VSStd2KCmdID.COMPLETEWORD:
                        prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_ENABLED | (uint)OLECMDF.OLECMDF_SUPPORTED;
                        return VSConstants.S_OK;
                }
            }

            return nextController.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            // If inside UI automation, promote execution to next controller.
            if (VsShellUtilities.IsInAutomationFunction(serviceProvider))
                return nextController.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            //make a copy of this so we can look at it after forwarding some commands 
            uint commandID = nCmdID;
            char typedChar = Char.MinValue;
            
            // Try to read input as char.
            if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
                typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);

            // Try start completion on 'Ctrl+Space'.
            if (nCmdID == (uint)VSConstants.VSStd2KCmdID.COMPLETEWORD)
            {
                if (!completionSession.HasSession)
                    completionSession.TryStartSession();

                completionSession.TryFilter();
                return VSConstants.S_OK;
            }

            // If we have active session.
            if (completionSession.HasSession)
            {
                // Try commit completion (Enter, Tab or Space).
                if (nCmdID == (uint)VSConstants.VSStd2KCmdID.RETURN || nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB || Char.IsWhiteSpace(typedChar))
                {
                    switch (completionSession.TryCommit())
                    {
                        case CompletionSession.CommitResult.Commited:
                        case CompletionSession.CommitResult.NoSession:
                            completionSession.TryDismiss();
                            return VSConstants.S_OK;
                        case CompletionSession.CommitResult.OtherMoniker:
                            return nextController.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
                    }
                }
            }

            // Let input be written into the buffer.
            int nextResult = nextController.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            // On text input, filter out completion.
            if (!typedChar.Equals(Char.MinValue))
            {
                SnapshotPoint cursorPosition = textView.Caret.Position.BufferPosition;
                string textContent = textView.TextBuffer.CurrentSnapshot.GetText();

                SyntaxNode currentNode = CSharpContentHelper.FindCurrentNode(cursorPosition, textContent);
                if (currentNode != null)
                {
                    LiteralExpressionSyntax literalNode = currentNode as LiteralExpressionSyntax;
                    if (literalNode != null)
                    {
                        string stringValue = literalNode.Token.Text as string;
                        if (stringValue != null)
                        {
                            // If this is not deletion, start session.
                            if (!completionSession.HasSession && commandID != (uint)VSConstants.VSStd2KCmdID.BACKSPACE && commandID != (uint)VSConstants.VSStd2KCmdID.DELETE)
                                completionSession.TryStartSession();

                            // Update filter.
                            completionSession.TryFilter();
                            return VSConstants.S_OK;
                        }
                    }
                }
            }
            
            // Return value from next controller.
            return nextResult;
        }
    }
}
