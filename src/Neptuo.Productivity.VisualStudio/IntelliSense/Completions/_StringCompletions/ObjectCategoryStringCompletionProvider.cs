﻿using Microsoft.VisualStudio.Language.Intellisense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense.Completions
{
    internal class ObjectCategoryStringCompletionProvider : ICSharpStringCompletionProvider
    {
        private readonly IGlyphService glyphService;
        private readonly List<string> completionItems = new List<string>() { "Person", "Organization", "BusinessCase", "GeneralTemplate", "BusinessCase.PriceItemJoiner" };

        public ObjectCategoryStringCompletionProvider(IGlyphService glyphService)
        {
            Ensure.NotNull(glyphService, "glyphService");
            this.glyphService = glyphService;
        }

        public IList<Completion> GetCompletionList(string textValue, string contextName)
        {
            textValue = textValue.ToLowerInvariant();

            return completionItems
                 .Where(s => s.ToLowerInvariant().StartsWith(textValue))
                 .Select(s => CreateCompletion(s, textValue))
                 .ToList();
        }

        private Completion CreateCompletion(string itemValue, string editorValue)
        {
            return new Completion(
                itemValue,
                itemValue.Substring(editorValue.Length),
                "",
                glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupMethod, StandardGlyphItem.GlyphItemPublic),
                ""
            );
        }
    }
}
