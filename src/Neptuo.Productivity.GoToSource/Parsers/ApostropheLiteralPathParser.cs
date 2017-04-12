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
    [Name(Name)]
    public class ApostropheLiteralPathParser : LiteralPathParser
    {
        public const string Name = "Apostrophe Literal Parser";

        public ApostropheLiteralPathParser() 
            : base("'")
        { }
    }
}
