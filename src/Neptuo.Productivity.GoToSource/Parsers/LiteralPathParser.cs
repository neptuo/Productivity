using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    public class LiteralPathParser : IPathParser
    {
        private readonly string separator;

        public LiteralPathParser(string separator)
        {
            Ensure.NotNull(separator, "separator");
            this.separator = separator;
        }

        public bool TryParse(string line, int index, out string path)
        {
            int indexOfStartQuote = line.IndexOf(separator, 0, index - 1);
            int indexOfEndQuote = line.IndexOf(separator, index);

            if (indexOfStartQuote >= 0 && indexOfEndQuote >= 0)
            {
                indexOfStartQuote++;
                path = line.Substring(indexOfStartQuote, indexOfEndQuote - indexOfStartQuote);
                return true;
            }

            path = null;
            return false;
        }
    }
}
