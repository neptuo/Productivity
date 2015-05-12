using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense.Completions
{
    [Order(Before = "default")]
    [Export(typeof(ICompletionSourceProvider))]
    [Name(ContentType)]
    [ContentType(ContentType)]
    internal class CSharpCompletionSourceProvider : ICompletionSourceProvider
    {
        public const string ContentType = "CSharp";

        [Import]
        public IGlyphService GlyphService { get; set; }

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new CSharpCompletionSource(
                textBuffer,
                new StringCompletionProviderCollection()
                    //.AddProvider("ObjectCategory", new ObjectCategoryStringCompletionProvider(GlyphService))
                    .AddProvider("Form", new FormStringCompletionProvider(GlyphService))
                    .AddProvider("ResourceItem", new ResourceStringCompletionProvider(GlyphService))
            );
        }
    }
}
