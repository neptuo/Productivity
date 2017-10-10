using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.ViewModels;
using Neptuo.Productivity.VisualStudio.Views.DesignData;
using System;
using System.Collections.Generic;
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

        public new AddNewItemView Content
        {
            get { return (AddNewItemView)base.Content; }
            set { base.Content = value; }
        }

        public AddNewItemWindow()
        {
            Caption = "Add new item...";
            Content = new AddNewItemView();
        }

        public override void OnToolWindowCreated()
        {
            base.OnToolWindowCreated();

            Content.DataContext = new MainViewModel(
                new DteFileService(
                    (DTE)GetService(typeof(DTE))
                )
            );
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
            EnsureHandle();
            if (m.HWnd == handle && m.Msg == WM_KEYDOWN && m.WParam.ToInt32() == VK_ESCAPE)
                Hide();

            return base.PreProcessMessage(ref m);
        }

        public void Hide()
        {
            ((IVsWindowFrame)Frame).Hide();
        }
    }
}
