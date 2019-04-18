using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public static class _TextSelectionExtensions
    {
        /// <summary>
        /// Adds at current position a new empty line.
        /// </summary>
        /// <param name="selection">A text selection.</param>
        public static void NewLineEmpty(this TextSelection selection)
        {
            selection.NewLine();
            selection.DeleteLineText();
        }

        /// <summary>
        /// Deletes all content/text from the current line.
        /// </summary>
        /// <param name="selection">A text selection.</param>
        public static void DeleteLineText(this TextSelection selection)
        {
            selection.EndOfLine();
            selection.DeleteLeft(selection.CurrentColumn - 1);
        }
    }
}
