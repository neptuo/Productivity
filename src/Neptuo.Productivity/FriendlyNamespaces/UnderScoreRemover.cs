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
    public class UnderScoreRemover
    {
        public void FixNamespace(string textContent)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(
                textContent, 
                new CSharpParseOptions(LanguageVersion.CSharp5, DocumentationMode.Parse, SourceCodeKind.Interactive)
            );

            SyntaxNode root = tree.GetRoot();
            foreach (NamespaceDeclarationSyntax namespaceSyntax in root.ChildNodes().OfType<NamespaceDeclarationSyntax>())
            {
                foreach (IdentifierNameSyntax identifierSyntax in namespaceSyntax.Name.ChildNodes())
                {
                    if(identifierSyntax.Identifier.Text.StartsWith("_"))
                    {
                        //identifierSyntax.ReplaceNode(identifierSyntax.Identifier, NameSyntax.)
                    }
                }
            }

            UnderScoreVisitor visitor = new UnderScoreVisitor();
            visitor.Visit(root);
        }
    }
}
