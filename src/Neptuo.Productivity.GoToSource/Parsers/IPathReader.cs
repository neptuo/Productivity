using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Parsers
{
    /// <summary>
    /// A path reader from opened text document.
    /// </summary>
    public interface IPathReader
    {
        /// <summary>
        /// Tries to read path from <paramref name="textDocument"/>.
        /// </summary>
        /// <param name="textDocument">A text document, where path is to find.</param>
        /// <param name="path">A found path.</param>
        /// <returns><c>true</c> if path was found; <c>false</c> otherwise.</returns>
        bool TryRead(TextDocument textDocument, out string path);
    }
}
