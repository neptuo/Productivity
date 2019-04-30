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
            if (TryGetSolutionPath(out string solutionPath))
                return Directory.Exists(Path.Combine(solutionPath, path));

            return true;
        }

        public bool FileExists(string path)
        {
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

        public void CreateFile(string filePath, Encoding encoding, string content)
        {
            FileInfo file = new FileInfo(filePath);
            filePath = file.FullName;

            object selectedItem = GetSelectedItem();
            Project project = GetSelectedProject(selectedItem);

            string fileName = Path.GetFileName(filePath);
            string folderPath = Path.GetDirectoryName(filePath);

            PackageUtilities.EnsureOutputPath(folderPath);

            try
            {
                // Create file and write content.
                using (FileStream fileContent = File.Create(filePath))
                {
                    ProjectItem projectItem = null;

                    if (selectedItem is ProjectItem parentItem && EnvDTE.Constants.vsProjectItemKindVirtualFolder == parentItem.Kind)
                        projectItem = parentItem.ProjectItems.AddFromFile(filePath);

                    if (projectItem == null)
                        projectItem = project.ProjectItems.AddFromFile(filePath);
                }

                using (FileStream fileContent = File.Create(filePath))
                using (StreamWriter writer = new StreamWriter(fileContent, encoding))
                    writer.Write(content);

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

        public void UpdateContent(string filePath, string content)
        {
            if (File.Exists(filePath))
            {
                if (VsShellUtilities.IsDocumentOpen(services, filePath, Guid.Empty, out IVsUIHierarchy hierarchy, out uint itemId, out IVsWindowFrame windowFrame))
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
            ProjectItem selectedItem = rawSelectedItem as ProjectItem;
            Project selectedProject = rawSelectedItem as Project;
            Project project = selectedItem?.ContainingProject ?? selectedProject ?? GetActiveProject();

            return project;
        }

        private Project GetActiveProject()
        {
            try
            {
                var activeSolutionProjects = dte.ActiveSolutionProjects as Array;

                if (activeSolutionProjects != null && activeSolutionProjects.Length > 0)
                    return activeSolutionProjects.GetValue(0) as Project;

                var doc = dte.ActiveDocument;

                if (doc != null && !string.IsNullOrEmpty(doc.FullName))
                {
                    var item = (dte.Solution != null) ? dte.Solution.FindProjectItem(doc.FullName) : null;

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
            IntPtr hierarchyPointer, selectionContainerPointer;
            object selectedObject = null;
            IVsMultiItemSelect multiItemSelect;
            uint itemId;

            var monitorSelection = (IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection));

            try
            {
                monitorSelection.GetCurrentSelection(out hierarchyPointer,
                                                 out itemId,
                                                 out multiItemSelect,
                                                 out selectionContainerPointer);

                IVsHierarchy selectedHierarchy = Marshal.GetTypedObjectForIUnknown(
                                                     hierarchyPointer,
                                                     typeof(IVsHierarchy)) as IVsHierarchy;

                if (selectedHierarchy != null)
                {
                    ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out selectedObject));
                }

                Marshal.Release(hierarchyPointer);
                Marshal.Release(selectionContainerPointer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
            }

            return selectedObject;
        }

        public static IWpfTextView FindCurentTextView()
        {
            var componentModel = GetComponentModel();
            if (componentModel == null) return null;
            var editorAdapter = componentModel.GetService<IVsEditorAdaptersFactoryService>();

            return editorAdapter.GetWpfTextView(GetCurrentNativeTextView());
        }

        public static IVsTextView GetCurrentNativeTextView()
        {
            var textManager = (IVsTextManager)ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager));

            IVsTextView activeView = null;
            ErrorHandler.ThrowOnFailure(textManager.GetActiveView(1, null, out activeView));
            return activeView;
        }

        public static IComponentModel GetComponentModel()
        {
            return (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
        }

        #endregion

        public void CreateDirectory(string path)
        {
            //if (file.FullName.EndsWith("__dummy__"))
            //{
            //    projectItem?.Delete();
            //    return;
            //}

            // TODO: Creating directories.
            throw Ensure.Exception.NotImplemented();
        }
    }
}
