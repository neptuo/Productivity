using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public static class _DteExtensions
    {
        public static TextDocument GetTextDocument(this Document document)
        {
            Ensure.NotNull(document, "document");
            return (TextDocument)document.Object("TextDocument");
        }
    }
}
