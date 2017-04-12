using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    /// <summary>
    /// An implementation of <see cref="IPathParser"/> which parses string literal wrapped by <c>"</c>.
    /// Eg. "~/Path".
    /// </summary>
    [Export(typeof(IPathParser))]
    [Name(Name)]
    public class QuoteLiteralPathParser : LiteralPathParser
    {
        public const string Name = "Quote Literal Parser";

        public QuoteLiteralPathParser()
            : base("\"")
        { }
    }
}
