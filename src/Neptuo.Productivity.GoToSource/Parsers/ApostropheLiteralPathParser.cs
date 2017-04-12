using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    [Export(typeof(IPathParser))]
    [Name(ParserName.Apostrophe)]
    public class ApostropheLiteralPathParser : LiteralPathParser
    {
        public ApostropheLiteralPathParser() 
            : base("'")
        { }
    }
}
