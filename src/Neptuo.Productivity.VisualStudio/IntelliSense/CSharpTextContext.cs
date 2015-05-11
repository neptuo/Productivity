using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Neptuo.Productivity.VisualStudio.IntelliSense.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense
{
    internal class CSharpTextContext
    {
        private readonly ITextView textView;

        public SyntaxNode CurrentNode { get; private set; }
        public string CurrentTextValue { get; private set; }

        public SyntaxNode ParentNode
        {
            get { return CurrentNode.Parent; }
        }

        public bool IsParentCastNode
        {
            get { return CurrentNode.Parent as CastExpressionSyntax != null; }
        }

        public bool IsCurrentTextNode
        {
            get { return CurrentTextValue != null; }
        }

        public CSharpTextContext(ITextView textView)
        {
            Ensure.NotNull(textView, "textView");
            this.textView = textView;
        }

        public void ResetCurrentNode()
        {
            CurrentNode = null;
            CurrentTextValue = null;

            SnapshotPoint cursorPosition = textView.Caret.Position.BufferPosition;
            string textContent = textView.TextBuffer.CurrentSnapshot.GetText();

            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                textContent,
                new CSharpParseOptions(LanguageVersion.CSharp5, DocumentationMode.Parse, SourceCodeKind.Interactive)
            );

            CSharpStringSyntaxVisitor visitor = new CSharpStringSyntaxVisitor(
                cursorPosition,
                (node, value) =>
                {
                    CurrentNode = node;
                    CurrentTextValue = value;
                }
            );
            visitor.Visit(tree.GetRoot());
        }
    }
}
