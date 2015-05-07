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
    internal class UnderScoreVisitor : CSharpSyntaxVisitor
    {
        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            base.VisitNamespaceDeclaration(node);
        }

        public override void DefaultVisit(SyntaxNode node)
        {
            base.DefaultVisit(node);
        }
    }
}
