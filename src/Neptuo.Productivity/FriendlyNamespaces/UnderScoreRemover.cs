using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.FriendlyNamespaces
{
    public class UnderscoreRemover
    {
        public string FixNamespace(string textContent)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                textContent, 
                new CSharpParseOptions(LanguageVersion.CSharp5, DocumentationMode.Parse, SourceCodeKind.Interactive)
            );

            UnderscoreRewriter rewriter = new UnderscoreRewriter();
            SyntaxNode root = tree.GetRoot();
            SyntaxNode newRoot = rewriter.Visit(root);

            if (rewriter.HasRewrites)
                return newRoot.NormalizeWhitespace().ToFullString();

            return textContent;
        }
    }
}
