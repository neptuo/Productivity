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
    internal class UnderscoreSyntaxRewriter : CSharpSyntaxRewriter
    {
        public bool HasRewrites { get; private set; }

        public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            NameSyntax result = null;

            List<string> resultNamespace = new List<string>();
            foreach (NameSyntax name in node.Name.ChildNodes())
                VisitName(name, resultNamespace);

            result = SyntaxFactory.IdentifierName(String.Join(".", resultNamespace)).WithTrailingTrivia(node.Name.GetTrailingTrivia());
            NamespaceDeclarationSyntax newNode = node.ReplaceNode(node.Name, result);
            return newNode;
        }

        private void VisitName(NameSyntax name, List<string> resultNamespace)
        {
            QualifiedNameSyntax qualifiedName = name as QualifiedNameSyntax;
            if (qualifiedName != null)
            {
                VisitQualifiedName(qualifiedName, resultNamespace);
                return;
            }

            IdentifierNameSyntax identifierName = name as IdentifierNameSyntax;
            if (identifierName != null)
            {
                VisitIdentifierName(identifierName, resultNamespace);
                return;
            }
        }

        private void VisitIdentifierName(IdentifierNameSyntax identifierName, List<string> resultNamespace)
        {
            if (!identifierName.Identifier.Text.StartsWith("_"))
            {
                resultNamespace.Add(identifierName.Identifier.Text);
                HasRewrites = true;
            }
        }

        private void VisitQualifiedName(QualifiedNameSyntax qualifiedName, List<string> resultNamespace)
        {
            VisitName(qualifiedName.Left, resultNamespace);
            VisitName(qualifiedName.Right, resultNamespace);
        }
    }
}
