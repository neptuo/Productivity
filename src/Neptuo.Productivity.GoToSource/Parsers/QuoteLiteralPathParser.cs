using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    [Export(typeof(IPathParser))]
    public class QuoteLiteralPathParser : LiteralPathParser
    {
        public QuoteLiteralPathParser()
            : base("\"")
        { }
    }
}
