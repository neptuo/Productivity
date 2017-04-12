using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Processors
{
    /// <summary>
    /// A path runners.
    /// </summary>
    public interface IPathProcessor
    {
        /// <summary>
        /// Opens whatever is at <paramref name="path"/>.
        /// </summary>
        /// <param name="path">A path to execute.</param>
        /// <returns><c>true</c> if succeeded; <c>false</c> it not.</returns>
        bool TryRun(string path);
    }
}
