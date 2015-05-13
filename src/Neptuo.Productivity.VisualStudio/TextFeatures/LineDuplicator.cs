using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.TextFeatures
{
    public class LineDuplicator
    {
        private readonly TextDocument textDocument;

        public LineDuplicator(TextDocument textDocument)
        {
            Ensure.NotNull(textDocument, "textDocument");
            this.textDocument = textDocument;
        }

        public void DuplicateCurrentLineDown()
        {
            DuplicateLineDown(textDocument.Selection.ActivePoint);
        }

        public void DuplicateLineDown(TextPoint lineToDuplicate)
        {
            DuplicateLine(lineToDuplicate, true);
        }

        public void DuplicateCurrentLineUp()
        {
            DuplicateLineUp(textDocument.Selection.ActivePoint);
        }

        public void DuplicateLineUp(TextPoint lineToDuplicate)
        {
            DuplicateLine(lineToDuplicate, false);
        }

        private void DuplicateLine(TextPoint lineToDuplicate, bool isDuplicationDown)
        {
            // Create edit point from original point.
            EditPoint originalPoint = textDocument.CreateEditPoint(lineToDuplicate);

            // Create line start point.
            EditPoint startLinePoint = textDocument.CreateEditPoint(lineToDuplicate);
            startLinePoint.StartOfLine();

            // Create line end point.
            EditPoint endLinePoint = textDocument.CreateEditPoint(lineToDuplicate);
            endLinePoint.EndOfLine();

            // Get line text content.
            string lineContent = startLinePoint.GetText(endLinePoint);

            // Create new line, empty it and insert text from previous/next line.
            if (!isDuplicationDown)
                textDocument.Selection.LineUp();
             
            textDocument.Selection.EndOfLine();
            textDocument.Selection.NewLine();
            textDocument.Selection.DeleteWhitespace();
            textDocument.Selection.Insert(lineContent);

            // Move original point to the new line.
            if (isDuplicationDown)
                originalPoint.LineDown();
            else
                originalPoint.LineUp();

            // Move to original column index on new line.
            textDocument.Selection.MoveToPoint(originalPoint);
        }
    }
}
