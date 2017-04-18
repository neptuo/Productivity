using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Utilities;

namespace Neptuo.Productivity.Parsers
{
    /// <summary>
    /// An implementation of <see cref="IPathReader"/> which tries to use exported <see cref="IPathParser"/> to find path in document current line.
    /// </summary>
    [Export(typeof(IPathReader))]
    [Name(Name)]
    public class DefaultPathReader : IPathReader
    {
        public const string Name = "Default Path Reader";

        [ImportMany]
        public IEnumerable<Lazy<IPathParser, IOrderable>> Parsers { get; set; }

        private bool isParsersOrdered;

        private void EnsureParsersOrder()
        {
            if (isParsersOrdered)
                return;

            Parsers = Orderer.Order(Parsers);
            isParsersOrdered = true;
        }

        public bool TryRead(TextDocument textDocument, out string path)
        {
            if (TryGetLineAt(textDocument, out string line, out int index))
            {
                EnsureParsersOrder();
                foreach (var parser in Parsers)
                {
                    if (parser.Value.TryParse(line, index, out path))
                        return true;
                }
            }

            path = null;
            return false;
        }

        private bool TryGetLineAt(TextDocument textDocument, out string content, out int index)
        {
            EditPoint currentPoint = textDocument.CreateEditPoint();

            EditPoint startLine = textDocument.CreateEditPoint(textDocument.Selection.TopPoint);
            startLine.StartOfLine();

            // Create line end point.
            EditPoint endLine = textDocument.CreateEditPoint(textDocument.Selection.TopPoint);
            endLine.EndOfLine();

            // Get line text content.
            content = startLine.GetText(endLine);
            index = textDocument.Selection.TopPoint.LineCharOffset;
            return true;
        }
    }
}
