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
    internal class CSharpStringContext
    {
        private readonly ITextView textView;

        public SyntaxNode CurrentNode { get; private set; }
        public string CurrentTextValue { get; private set; }

        public SyntaxNode ParentNode
        {
            get { return CurrentNode.Parent; }
        }

        public string ParentCastTypeName { get; private set; }

        public bool IsParentCastNode
        {
            get { return CurrentNode.Parent as CastExpressionSyntax != null; }
        }

        public bool IsCurrentTextNode
        {
            get { return CurrentTextValue != null; }
        }

        public int CursorPosition { get; private set; }

        public int CurrentTextNodePosition
        {
            get
            {
                if (!IsCurrentTextNode)
                    throw Ensure.Exception.InvalidOperation("Unnable to get current text position when not inside text node.");

                return CurrentNode.SpanStart;
            }
        }

        public CSharpStringContext(ITextView textView)
        {
            Ensure.NotNull(textView, "textView");
            this.textView = textView;

            ResetCurrentNode();
        }

        public void ResetCurrentNode()
        {
            CurrentNode = null;
            CurrentTextValue = null;
            ParentCastTypeName = null;
            
            CursorPosition = textView.Caret.Position.BufferPosition;
            string textContent = textView.TextBuffer.CurrentSnapshot.GetText();

            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                textContent,
                new CSharpParseOptions(LanguageVersion.CSharp5, DocumentationMode.Parse, SourceCodeKind.Interactive)
            );

            CSharpStringCastSyntaxVisitor visitor = new CSharpStringCastSyntaxVisitor(
                CursorPosition,
                (node, value) =>
                {
                    CurrentNode = node;
                    CurrentTextValue = value;

                    CastExpressionSyntax castParentNode = node.Parent as CastExpressionSyntax;
                    if (castParentNode != null)
                    {
                        ParentCastTypeName = castParentNode.Type.ToString();
                    }
                }
            );
            visitor.Visit(tree.GetRoot());
        }
    }
}
