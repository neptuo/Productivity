using EnvDTE;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    [Export(typeof(IPathReader))]
    [Name(Name)]
    [Order(Before = DefaultPathReader.Name)]
    public class SelectionPathReader : IPathReader
    {
        public const string Name = "Selection Path Reader";

        public bool TryRead(TextDocument textDocument, out string path)
        {
            EditPoint currentPoint = textDocument.CreateEditPoint(textDocument.Selection.TopPoint);
            path = currentPoint.GetText(textDocument.Selection.BottomPoint);
            return !String.IsNullOrEmpty(path) && !String.IsNullOrWhiteSpace(path);
        }
    }
}
