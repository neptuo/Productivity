using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Tags;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neptuo.Productivity.IntelliSense
{
    [ExportCompletionProvider(nameof(GuidCompletionProvider), LanguageNames.CSharp)]
    internal class GuidCompletionProvider : CompletionProvider
    {
        public static class Property
        {
            public const string Guid = "Guid";
        }

        public override async Task ProvideCompletionsAsync(CompletionContext context)
        {
            if (context.Document.TryGetSyntaxTree(out SyntaxTree tree))
            {
                SyntaxNode root = await tree.GetRootAsync();
                SyntaxNode node = root.FindNode(context.CompletionListSpan);
                GuidInsertionType insertionType = IsGuid(node);
                if (insertionType != GuidInsertionType.None)
                    context.AddItem(CreateCompletionItem(insertionType));
            }
        }

        public override Task<CompletionDescription> GetDescriptionAsync(Document document, CompletionItem item, CancellationToken cancellationToken)
        {
            if (item.Properties.TryGetValue(Property.Guid, out string value))
                return Task.FromResult(CompletionDescription.FromText(value));

            return base.GetDescriptionAsync(document, item, cancellationToken);
        }

        private static CompletionItem CreateCompletionItem(GuidInsertionType insertionType)
        {
            string value = Guid.NewGuid().ToString().ToLower();

            string insertionText;
            switch (insertionType)
            {
                case GuidInsertionType.Constructor:
                    insertionText = $"Guid(\"{value}\")";
                    break;

                case GuidInsertionType.ValueWithQuotes:
                    insertionText = $"\"{value}\"";
                    break;

                case GuidInsertionType.Value:
                    insertionText = value;
                    break;

                default:
                    throw Ensure.Exception.NotSupported($"Not supported value '{insertionType}'.");
            }

            var tags = ImmutableArray.Create(WellKnownTags.Structure);
            var properties = ImmutableDictionary.Create<string, string>().Add(Property.Guid, value);
            var rules = CompletionItemRules.Create(matchPriority: MatchPriority.Preselect);

            CompletionItem item = CompletionItem.Create(
                insertionText,
                insertionText,
                insertionText,
                tags: tags,
                properties: properties,
                rules: rules
            );
            return item;
        }

        private static GuidInsertionType IsGuid(SyntaxNode node)
        {
            if (node is ObjectCreationExpressionSyntax objectCreation)
            {
                if (objectCreation.Parent is EqualsValueClauseSyntax equals)
                {
                    if (equals.Parent is VariableDeclaratorSyntax variableDeclarator)
                    {
                        if (variableDeclarator.Parent is VariableDeclarationSyntax variableDeclaration)
                        {
                            if (IsGuid(variableDeclaration.Type))
                                return GuidInsertionType.Constructor;
                        }
                    }
                    else if (equals.Parent is PropertyDeclarationSyntax propertyDeclaration)
                    {
                        if (IsGuid(propertyDeclaration.Type))
                            return GuidInsertionType.Constructor;
                    }
                }
            }
            else if (node is ArgumentListSyntax argumentList)
            {
                if (argumentList.Parent is ObjectCreationExpressionSyntax objectCreation1)
                {
                    if (IsGuid(objectCreation1.Type))
                        return GuidInsertionType.ValueWithQuotes;
                }
            }
            else if (node is ArgumentSyntax argument)
            {
                if (argument.Expression is LiteralExpressionSyntax literalExpression)
                {
                    if (literalExpression.Kind() == SyntaxKind.StringLiteralExpression && literalExpression.Token.ValueText == String.Empty)
                    {
                        if (argument.Parent is ArgumentListSyntax argumentList1)
                        {
                            if (argumentList1.Parent is ObjectCreationExpressionSyntax objectCreation2)
                            {
                                if (IsGuid(objectCreation2.Type))
                                    return GuidInsertionType.Value;
                            }
                        }
                    }
                }
            }

            return GuidInsertionType.None;
        }

        private static bool IsGuid(TypeSyntax type)
        {
            if (type is IdentifierNameSyntax identifierName)
            {
                string name = identifierName.Identifier.ValueText;
                if (name == nameof(Guid) || name == typeof(Guid).FullName)
                    return true;
            }

            return false;
        }
    }
}
