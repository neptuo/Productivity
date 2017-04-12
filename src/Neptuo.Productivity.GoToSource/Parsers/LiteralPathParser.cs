using Neptuo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    /// <summary>
    /// An implementation of <see cref="IPathParser"/> which parses string literals.
    /// Eg. "~/Path".
    /// </summary>
    public class LiteralPathParser : IPathParser
    {
        private readonly string separator;

        /// <summary>
        /// Creates a new instance with <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">A string to determine start and end of literal.</param>
        public LiteralPathParser(string separator)
        {
            Ensure.NotNull(separator, "separator");
            this.separator = separator;
        }

        public bool TryParse(string line, int index, out string path)
        {
            int indexOfStartQuote = line.Substring(0, index).LastIndexOf(separator);
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
