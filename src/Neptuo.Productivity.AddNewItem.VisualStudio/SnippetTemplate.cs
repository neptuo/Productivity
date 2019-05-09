using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neptuo.Collections.Specialized;

namespace Neptuo.Productivity
{
    public abstract class SnippetTemplate : ITemplate, IContentTemplate
    {
        public const string CursorEndToken = "$end$";

        public abstract Encoding Encoding { get; }

        public TemplateContent GetContent(IReadOnlyKeyValueCollection parameters)
        {
            string template = GetContent();

            foreach (string key in parameters.Keys)
                template = template.Replace($"${key}$", parameters.Get<string>(key));

            int cursor;
            (template, cursor) = ReplaceEndToken(template);

            return new TemplateContent(Encoding, template, cursor);
        }

        private static (string template, int cursor) ReplaceEndToken(string template)
        {
            int cursor = template.IndexOf(CursorEndToken);
            if (cursor >= 0)
                template = template.Replace(CursorEndToken, String.Empty);
            else
                cursor = 0;

            return (template, cursor);
        }

        protected abstract string GetContent();
    }
}
