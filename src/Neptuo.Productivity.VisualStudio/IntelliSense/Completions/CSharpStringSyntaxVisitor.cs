using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense.Completions
{
    public class CSharpStringSyntaxVisitor : CSharpSyntaxRewriter
    {
        private int position;
        private Action<LiteralExpressionSyntax, string> onNodeFound;

        public CSharpStringSyntaxVisitor(int position, Action<LiteralExpressionSyntax, string> onNodeFound)
        {
            this.position = position;
            this.onNodeFound = onNodeFound;
        }

        public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if (onNodeFound == null)
                return node;

            if (node.Span.Start < position && position < node.Span.Start + node.Span.Length)
            {
                CastExpressionSyntax expressionParent = node.Parent as CastExpressionSyntax;
                if(expressionParent != null) 
                {
                    string stringValue = node.Token.Value as string;
                    if (stringValue != null)
                    {
                        onNodeFound(node, stringValue);
                        onNodeFound = null;
                    }
                }
            }
            return node;
        }
    }
}
