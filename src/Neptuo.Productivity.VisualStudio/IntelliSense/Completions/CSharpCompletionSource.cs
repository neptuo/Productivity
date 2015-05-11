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
        private readonly List<string> completionItems = new List<string>() { "Person", "Person.List", "Person.Edit", "BusinessCase", "BusinessCase.List", "BusinessCase.Edit" };

        public CSharpCompletionSource(ITextBuffer textBuffer)
        {
            Ensure.NotNull(textBuffer, "textBuffer");
            this.textBuffer = textBuffer;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            SnapshotPoint cursorPosition = session.TextView.Caret.Position.BufferPosition;
            string textContent = textBuffer.CurrentSnapshot.GetText();

            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                textContent,
                new CSharpParseOptions(LanguageVersion.CSharp5, DocumentationMode.Parse, SourceCodeKind.Interactive)
            );

            SyntaxNode currentNode = tree
                .GetRoot()
                .ChildNodes()
                .FirstOrDefault(n => n.Span.Start < cursorPosition && cursorPosition < n.Span.Start + n.Span.Length);

            List<Completion> result = new List<Completion>();
            CSharpStringSyntaxVisitor visitor = new CSharpStringSyntaxVisitor(
                cursorPosition, 
                (stringValue) =>
                {
                    result.AddRange(
                        completionItems
                            .Where(s => s.StartsWith(stringValue))
                            .Select(s => new Completion(s, s.Substring(stringValue.Length), "", null, ""))
                    );
                }
            );

            SyntaxNode root = tree.GetRoot();
            visitor.Visit(root);
            
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
