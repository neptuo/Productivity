using Neptuo.Collections.Specialized;
using Neptuo.Productivity.VisualStudio.ViewModels;
using Neptuo.Text.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public abstract class TokenTemplate : ITemplate
    {
        public const string CursorEndToken = "{Cursor.End}";

        public abstract Encoding Encoding { get; }

        public TemplateContent GetContent(IReadOnlyKeyValueCollection parameters)
        {
            string template = GetContent();
            int cursor = template.IndexOf(CursorEndToken);
            if (cursor >= 0)
                template = template.Replace(CursorEndToken, String.Empty);
            else
                cursor = 0;

            TokenWriter writer = new TokenWriter(template);
            string content = writer.Format(parameters);
            return new TemplateContent(Encoding, template, cursor);
        }

        protected abstract string GetContent();
    }
}
