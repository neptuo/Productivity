using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Processors.Mappers
{
    /// <summary>
    /// Extends source (path) by some additional/context information.
    /// Eg.: Can map relative paths to absolute.
    /// </summary>
    public interface IPathMapper
    {
        /// <summary>
        /// Modifies <paramref name="source"/> to result path.
        /// </summary>
        /// <param name="source">A path to extend.</param>
        /// <returns>A modified path or <paramref name="source"/>.</returns>
        string Map(string source);
    }
}
