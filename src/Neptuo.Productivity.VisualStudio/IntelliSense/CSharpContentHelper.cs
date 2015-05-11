using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense
{
    internal class CSharpContentHelper
    {
        public static SyntaxNode FindCurrentNode(int cursorPosition, string textContent)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                textContent,
                new CSharpParseOptions(LanguageVersion.CSharp5, DocumentationMode.Parse, SourceCodeKind.Interactive)
            );

            SyntaxNode currentNode = tree
                .GetRoot()
                .ChildNodes()
                .FirstOrDefault(n => n.Span.Start < cursorPosition && cursorPosition < n.Span.Start + n.Span.Length);
            
            return currentNode;
        }
    }
}
