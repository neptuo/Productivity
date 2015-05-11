using Microsoft.VisualStudio.Language.Intellisense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.IntelliSense.Completions
{
    public class StringCompletionProviderCollection : ICSharpStringCompletionProvider
    {
        private readonly Dictionary<string, ICSharpStringCompletionProvider> storage = new Dictionary<string, ICSharpStringCompletionProvider>();

        public StringCompletionProviderCollection AddProvider(string contextName, ICSharpStringCompletionProvider provider)
        {
            Ensure.NotNull(contextName, "contextName");
            Ensure.NotNull(provider, "provider");
            storage[contextName] = provider;
            return this;
        }

        public IList<Completion> GetCompletionList(string textValue, string contextName)
        {
            ICSharpStringCompletionProvider provider;
            if (storage.TryGetValue(contextName, out provider))
                return provider.GetCompletionList(textValue, contextName);

            return new List<Completion>();
        }
    }
}
