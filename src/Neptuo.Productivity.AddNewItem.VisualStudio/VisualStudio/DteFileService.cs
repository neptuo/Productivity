using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Neptuo;
using Neptuo.Collections.Specialized;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    public class DteFileService : IFileService
    {
        private readonly DTE dte;
        private readonly IServiceProvider services;

        public DteFileService(DTE dte, IServiceProvider services)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(services, "services");
            this.dte = dte;
            this.services = services;
        }

        private bool TryGetSolutionPath(out string solutionPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (dte.Solution == null)
            {
                solutionPath = null;
                return false;
            }

            solutionPath = Path.GetDirectoryName(dte.Solution.FileName);
            return true;
        }

        public bool DirectoryExists(string path)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (TryGetSolutionPath(out string solutionPath))
                return Directory.Exists(Path.Combine(solutionPath, path));

            return true;
        }

        public bool FileExists(string path)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (TryGetSolutionPath(out string solutionPath))
                return File.Exists(Path.Combine(solutionPath, path));

            return true;
        }

        public bool IsValidFileName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            if (name.Contains(Path.DirectorySeparatorChar) || name.Contains(Path.AltDirectorySeparatorChar))
            {
                string path = Path.GetDirectoryName(name);
                if (!IsValidDirectoryName(path))
                    return false;

                name = Path.GetFileName(name);
            }

            if (PackageUtilities.IsFileNameInvalid(name))
                return false;

            return true;
        }

        public bool IsValidDirectoryName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            foreach (char item in Path.GetInvalidPathChars())
            {
                if (name.Contains(item))
                    return false;
            }

            return true;
        }

        public void CreateDirectory(string path)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ProjectItem projectItem = CreateProjectItem(Path.Combine(path, "__dummy__"), Encoding.UTF8, String.Empty);
            if (projectItem != null)
                projectItem.Delete();
        }

        public void CreateFile(string filePath, Encoding encoding, string content)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                CreateProjectItem(filePath, encoding, content);

                // Open document.
                VsShellUtilities.OpenDocument(services, filePath);
                dte.ActiveDocument.Activate();
            }
            catch (Exception ex)
            {
                // TODO: Handle exceptions.
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        private ProjectItem CreateProjectItem(string filePath, Encoding encoding, string content)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            FileInfo file = new FileInfo(filePath);
            filePath = file.FullName;

            object selectedItem = GetSelectedItem();
            Project project = GetSelectedProject(selectedItem);

            string folderPath = Path.GetDirectoryName(filePath);

            PackageUtilities.EnsureOutputPath(folderPath);

            ProjectItem projectItem = null;

            using (FileStream fileContent = File.Create(filePath))
            {
                if (selectedItem is ProjectItem parentItem && EnvDTE.Constants.vsProjectItemKindVirtualFolder == parentItem.Kind)
                    projectItem = parentItem.ProjectItems.AddFromFile(filePath);

                if (projectItem == null)
                    projectItem = project.ProjectItems.AddFromFile(filePath);

                // Create file and write content.
                using (StreamWriter writer = new StreamWriter(fileContent, encoding))
                    writer.Write(content);

                return projectItem;
            }
        }

        public void UpdateContent(string filePath, string content)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (File.Exists(filePath))
            {
                bool isOpened = VsShellUtilities.IsDocumentOpen(
                    services, 
                    filePath, 
                    Guid.Empty, 
                    out IVsUIHierarchy hierarchy, 
                    out uint itemId, 
                    out IVsWindowFrame windowFrame
                );

                if (isOpened)
                {
                    // TODO: Update content and save.
                }
                else
                {
                    using (FileStream fileContent = File.OpenWrite(filePath))
                    using (StreamWriter writer = new StreamWriter(fileContent))
                    {
                        writer.Write(content);
                    }
                }
            }
            else
            {
                // TODO: Create a new file.
            }
        }

        #region Helpers

        private Project GetSelectedProject(object rawSelectedItem)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ProjectItem selectedItem = rawSelectedItem as ProjectItem;
            Project selectedProject = rawSelectedItem as Project;
            Project project = selectedItem?.ContainingProject ?? selectedProject ?? FindActiveProject();

            return project;
        }

        private Project FindActiveProject()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                if (dte.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0)
                    return activeSolutionProjects.GetValue(0) as Project;

                var doc = dte.ActiveDocument;

                if (doc != null && !string.IsNullOrEmpty(doc.FullName))
                {
                    var item = dte.Solution?.FindProjectItem(doc.FullName);

                    if (item != null)
                        return item.ContainingProject;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        public static object GetSelectedItem()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            object selectedObject = null;
            var monitorSelection = (IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection));

            try
            {
                monitorSelection.GetCurrentSelection(
                    out IntPtr hierarchyPointer,
                    out uint itemId,
                    out IVsMultiItemSelect multiItemSelect,
                    out IntPtr selectionContainerPointer
                );

                IVsHierarchy selectedHierarchy = (IVsHierarchy)Marshal.GetTypedObjectForIUnknown(hierarchyPointer, typeof(IVsHierarchy));
                if (selectedHierarchy != null)
                    ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out selectedObject));

                Marshal.Release(hierarchyPointer);
                Marshal.Release(selectionContainerPointer);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }

            return selectedObject;
        }

        public IWpfTextView FindCurentTextView()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var componentModel = GetComponentModel();
            if (componentModel == null) return null;
            var editorAdapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();

            return editorAdapter.GetWpfTextView(GetCurrentNativeTextView());
        }

        public IVsTextView GetCurrentNativeTextView()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var textManager = (IVsTextManager)Package.GetGlobalService(typeof(SVsTextManager));
            ErrorHandler.ThrowOnFailure(textManager.GetActiveView(1, null, out IVsTextView activeView));
            return activeView;
        }

        public static IComponentModel GetComponentModel() => (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));

        #endregion
    }
}
