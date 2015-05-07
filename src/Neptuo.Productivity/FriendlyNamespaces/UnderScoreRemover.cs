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
                List<IdentifierNameSyntax> toRemove = new List<IdentifierNameSyntax>();
                foreach (IdentifierNameSyntax identifierSyntax in namespaceSyntax.Name.ChildNodes())
                {
                    
                    if(identifierSyntax.Identifier.Text.StartsWith("_"))
                        toRemove.Add(identifierSyntax);
                }

                //TODO: Use SyntaxFactory....
                NameSyntax result = namespaceSyntax.Name;
                foreach (IdentifierNameSyntax item in toRemove)
                    result = result.RemoveNode(item, SyntaxRemoveOptions.KeepNoTrivia);

                NamespaceDeclarationSyntax newNamespaceSyntax = namespaceSyntax.ReplaceNode(namespaceSyntax.Name, result);
            }

            UnderScoreVisitor visitor = new UnderScoreVisitor();
            visitor.Visit(root);
        }
    }
}
