using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    /// <summary>
    /// A path parsing component.
    /// </summary>
    public interface IPathParser
    {
        /// <summary>
        /// Tries to parse <paramref name="line"/> to whatever <paramref name="path"/> is.
        /// </summary>
        /// <param name="line">A whole source line.</param>
        /// <param name="index">An index where cursor is.</param>
        /// <param name="path">An result path parsed from <paramref name="line"/>.</param>
        /// <returns><c>true</c> if succeeded; <c>false</c> it not.</returns>
        bool TryParse(string line, int index, out string path);
    }
}
