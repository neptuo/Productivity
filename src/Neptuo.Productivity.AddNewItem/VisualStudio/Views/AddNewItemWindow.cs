using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.ViewModels;
using Neptuo.Productivity.VisualStudio.Views.DesignData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace Neptuo.Productivity.VisualStudio.Views
{
    [Guid("088f3f0e-a9e1-4e31-b77b-69e6d0fa5294")]
    public class AddNewItemWindow : ToolWindowPane
    {
        private const int VK_ESCAPE = 0x1b;
        private const int WM_KEYDOWN = 0x100;

        private IntPtr handle;
        private DTE dte;
        private SelectionEvents events;

        public new AddNewItemView Content
        {
            get { return (AddNewItemView)base.Content; }
            set { base.Content = value; }
        }

        public MainViewModel ViewModel
        {
            get { return Content.ViewModel; }
        }

        public AddNewItemWindow()
        {
            Caption = "Add new item...";
            Content = new AddNewItemView();
        }

        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();

            dte = (DTE)GetService(typeof(DTE));

            events = dte.Events.SelectionEvents;
            events.OnChange += OnSelectionChanged;

            Content.DataContext = new MainViewModel(new DteFileService(dte, this), new XmlTemplateServiceFactory(), OnItemAdded);
            OnSelectionChanged();
        }

        private void OnItemAdded()
        {
            ViewModel.Name = null;
            Hide();
        }

        private void OnSelectionChanged()
        {
            if (dte.SelectedItems.Count == 1)
            {
                SelectedItem item = dte.SelectedItems.Item(1);
                string path = null;
                if (item.ProjectItem != null)
                    path = item.ProjectItem.FileNames[0];
                else if (item.Project != null)
                    path = Path.GetDirectoryName(item.Project.FileName);
                else if (dte.Solution != null)
                    path = Path.GetDirectoryName(dte.Solution.FileName);

                if (path != null)
                {
                    if (File.Exists(path))
                        path = Path.GetDirectoryName(path);

                    ViewModel.Path = path;
                }
            }
        }

        private void EnsureHandle()
        {
            if (handle == IntPtr.Zero)
            {
                HwndSource source = (HwndSource)PresentationSource.FromVisual(Content);
                if (source != null)
                    handle = source.Handle;
            }
        }

        protected override bool PreProcessMessage(ref Message m)
        {
            //EnsureHandle();
            if (((IVsWindowFrame)Frame).IsVisible() == 0 && m.Msg == WM_KEYDOWN && m.WParam.ToInt32() == VK_ESCAPE)
            {
                if (String.IsNullOrEmpty(ViewModel.Name))
                {
                    Hide();
                }
                else
                {
                    ViewModel.Name = null;
                    Show();
                }

                return true;
            }
            else
            {
                return base.PreProcessMessage(ref m);
            }
        }

        public void Show()
        {
            ((IVsWindowFrame)Frame).Show();
        }

        public void Hide()
        {
            ((IVsWindowFrame)Frame).Hide();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            events.OnChange -= OnSelectionChanged;
        }
    }
}
