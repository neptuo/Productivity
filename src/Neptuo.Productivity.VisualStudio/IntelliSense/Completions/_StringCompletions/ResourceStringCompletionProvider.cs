using Microsoft.VisualStudio.Language.Intellisense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense.Completions
{
    public class ResourceStringCompletionProvider : ICSharpStringCompletionProvider
    {
        private readonly IGlyphService glyphService;
        private readonly List<string> completionItems = new List<string>() { "ClientFramework", "ClientServices", "ClientWeb", "ClientBusinessCase", "mscorelib", "jQuery", "jQueryAutosize", "jQueryAutoComplete", "jQueryBeautyTips", "jQueryContextMenu", "jQueryTimers" };

        public ResourceStringCompletionProvider(IGlyphService glyphService)
        {
            Ensure.NotNull(glyphService, "glyphService");
            this.glyphService = glyphService;
        }

        public IList<Completion> GetCompletionList(string textValue, string contextName)
        {
            throw new NotImplementedException();
        }

        private Completion CreateCompletion(string itemValue, string editorValue)
        {
            return new Completion(
                itemValue,
                itemValue.Substring(editorValue.Length),
                "",
                glyphService.GetGlyph(StandardGlyphGroup.GlyphReference, StandardGlyphItem.GlyphItemPublic),
                ""
            );
        }
    }
}
