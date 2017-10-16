using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neptuo.Collections.Specialized;
using Neptuo.Text.Tokens;

namespace Neptuo.Productivity
{
    public abstract class TokenTemplate : ITemplate
    {
        public abstract Encoding Encoding { get; }

        public TemplateContent GetContent(IReadOnlyKeyValueCollection parameters)
        {
            TokenWriter writer = new TokenWriter(GetContent());
            string content = writer.Format(parameters);
            // TODO: Add support Cursor.
            return new TemplateContent(content);
        }

        protected abstract string GetContent();
    }
}
