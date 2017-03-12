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
            DuplicateLineDown(textDocument.Selection.TopPoint, textDocument.Selection.BottomPoint);
        }

        public void DuplicateLineDown(TextPoint startLine, TextPoint endLine)
        {
            DuplicateLine(startLine, endLine, true);
        }

        public void DuplicateCurrentLineUp()
        {
            DuplicateLineUp(textDocument.Selection.TopPoint, textDocument.Selection.BottomPoint);
        }

        public void DuplicateLineUp(TextPoint startLine, TextPoint endLine)
        {
            DuplicateLine(startLine, endLine, false);
        }

        private void DuplicateLine(TextPoint startLine, TextPoint endLine, bool isDuplicationDown)
        {
            using (new UndoContextDisposable(startLine.DTE, "lineduplicator"))
            {
                // Create edit point from original point.
                EditPoint originalPoint = textDocument.CreateEditPoint(startLine);

                // Create line start point.
                EditPoint startLinePoint = textDocument.CreateEditPoint(startLine);
                startLinePoint.StartOfLine();

                // Create line end point.
                EditPoint endLinePoint = textDocument.CreateEditPoint(endLine);
                endLinePoint.EndOfLine();

                // Get line text content.
                string lineContent = startLinePoint.GetText(endLinePoint);

                // Create new line, empty it and insert text from previous/next line.
                if (!isDuplicationDown)
                    textDocument.Selection.LineUp();

                textDocument.Selection.MoveToPoint(endLinePoint);
                textDocument.Selection.EndOfLine();
                textDocument.Selection.NewLine();
                textDocument.Selection.DeleteLeft(textDocument.Selection.ActivePoint.DisplayColumn - 1);
                textDocument.Selection.DeleteWhitespace();
                textDocument.Selection.Insert(lineContent);

                if (startLinePoint.Line == endLinePoint.Line)
                {
                    // Move original point to the new line.
                    if (isDuplicationDown)
                        originalPoint.LineDown();

                    // Move to original column index on new line.
                    textDocument.Selection.MoveToPoint(originalPoint);

                    if (originalPoint.DisplayColumn != endLine.DisplayColumn)
                        textDocument.Selection.MoveToPoint(endLine, true);
                }
                else
                {
                    // When duplicating multiple lines, select them all.
                    int lineCount = endLinePoint.Line - startLinePoint.Line;

                    if (isDuplicationDown)
                        originalPoint.LineDown(lineCount + 1);

                    originalPoint.StartOfLine();
                    textDocument.Selection.EndOfLine();
                    textDocument.Selection.MoveToPoint(originalPoint, true);
                }
            }
        }
    }
}
