using EnvDTE;
using System;

namespace Neptuo.Productivity.VisualStudio
{
    /// <summary>
    /// A line duplication service.
    /// </summary>
    public class LineDuplicator
    {
        private readonly TextDocument textDocument;

        /// <summary>
        /// Creates a new instance that duplicates lines in the <paramref name="textDocument"/>.
        /// </summary>
        /// <param name="textDocument">A document to duplicate lines in.</param>
        public LineDuplicator(TextDocument textDocument)
        {
            Ensure.NotNull(textDocument, "textDocument");
            this.textDocument = textDocument;
        }

        /// <summary>
        /// Duplicates current line (or selected lines) down.
        /// </summary>
        public void DuplicateCurrentLineDown() => DuplicateLineDown(textDocument.Selection.TopPoint, textDocument.Selection.BottomPoint);

        public void DuplicateLineDown(TextPoint startPoint, TextPoint endPoint) => DuplicateLine(startPoint, endPoint, true);

        /// <summary>
        /// Duplicates current line (or selected lines) up.
        /// </summary>
        public void DuplicateCurrentLineUp() => DuplicateLineUp(textDocument.Selection.TopPoint, textDocument.Selection.BottomPoint);

        public void DuplicateLineUp(TextPoint startPoint, TextPoint endPoint) => DuplicateLine(startPoint, endPoint, false);

        private void DuplicateLine(TextPoint startPoint, TextPoint endPoint, bool isDuplicationDown)
        {
            // Take the positions before any modification.
            int startPointColumn = startPoint.LineCharOffset;
            int startPointLine = startPoint.Line;
            int endPointColumn = endPoint.LineCharOffset;
            int endPointLine = endPoint.Line;
            int activePointColumn = textDocument.Selection.ActivePoint.LineCharOffset;

            string name = String.Format("Duplicate Current Line {0}", isDuplicationDown ? "Down" : "Up");
            using (new UndoContextDisposable(startPoint.DTE, name))
            {
                // Create edit point from original point.
                EditPoint originalPoint = textDocument.CreateEditPoint(startPoint);

                // Create line start point.
                EditPoint startLine = textDocument.CreateEditPoint(startPoint);
                startLine.StartOfLine();

                // Create line end point.
                EditPoint endLine = textDocument.CreateEditPoint(endPoint);
                endLine.EndOfLine();

                // Get line text content.
                string textContent = startLine.GetText(endLine);

                // Create new line, empty it and insert text from previous/next line.
                if (!isDuplicationDown)
                    textDocument.Selection.LineUp();

                textDocument.Selection.MoveToPoint(endLine);
                textDocument.Selection.EndOfLine();
                textDocument.Selection.NewLine();
                textDocument.Selection.DeleteWhitespace();
                textDocument.Selection.Insert(textContent);


                // Move original selection to the new text.
                int lineCount = endLine.Line - startLine.Line + 1;

                EditPoint startPointEdit = textDocument.CreateEditPoint(startPoint);
                startPointEdit.MoveToLineAndOffset(startPointLine, startPointColumn);

                EditPoint endPointEdit = textDocument.CreateEditPoint(endPoint);
                endPointEdit.MoveToLineAndOffset(endPointLine, endPointColumn);

                // If the selection if from end to start, we need to switch the points.
                if (activePointColumn == startPointColumn)
                {
                    EditPoint transfer = endPointEdit;
                    endPointEdit = startPointEdit;
                    startPointEdit = transfer;
                }

                // If duplication down, move to the new lines.
                if (isDuplicationDown)
                {
                    startPointEdit.LineDown(lineCount);
                    endPointEdit.LineDown(lineCount);
                }

                // Create new selection.
                textDocument.Selection.MoveToPoint(startPointEdit);
                textDocument.Selection.MoveToPoint(endPointEdit, true);
            }
        }
    }
}
