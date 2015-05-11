using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Neptuo.ComponentModel;
using Neptuo.Productivity.VisualStudio.IntelliSense.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense.Completions
{
    public class CSharpCompletionSource : DisposableBase, ICompletionSource
    {
        private readonly ITextBuffer textBuffer;
        private readonly ICSharpStringCompletionProvider stringCompletionProvider;

        public CSharpCompletionSource(ITextBuffer textBuffer, ICSharpStringCompletionProvider stringCompletionProvider)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            Ensure.NotNull(stringCompletionProvider, "stringCompletionProvider");
            this.textBuffer = textBuffer;
            this.stringCompletionProvider = stringCompletionProvider;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            CSharpStringContext context = new CSharpStringContext(session.TextView);
            List<Completion> result = new List<Completion>();

            if (context.IsCurrentTextNode && context.IsParentCastNode)
            {
                result.AddRange(
                    stringCompletionProvider.GetCompletionList(
                        context.CurrentTextValue, 
                        context.ParentCastTypeName
                    )
                );
            }

            CompletionSet newCompletionSet = new CompletionSet(
                "N:Productivity",
                "Neptuo Productivity",
                FindTokenSpanAtPosition(session.GetTriggerPoint(textBuffer), session),
                result,
                null
            );

            completionSets.Add(newCompletionSet);
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            SnapshotPoint currentPoint = session
                .GetTriggerPoint(textBuffer)
                .GetPoint(textBuffer.CurrentSnapshot);

            return currentPoint.Snapshot.CreateTrackingSpan(
                currentPoint.Position,
                0,
                SpanTrackingMode.EdgeInclusive
            );
        }
    }
}
