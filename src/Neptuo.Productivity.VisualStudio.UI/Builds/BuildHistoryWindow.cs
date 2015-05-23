using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.Specialized;

namespace Neptuo.Productivity.VisualStudio.UI.Builds
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("04576E43-F435-4C2B-A2EE-B56B9C3941DA")]
    public class BuildHistoryWindow : ToolWindowPane
    {
        protected BuildHistoryControl ContentControl
        {
            get { return (BuildHistoryControl)Content; }
            set { Content = value; }
        }

        public BuildHistoryViewModel ViewModel
        {
            get { return ContentControl.DataContext as BuildHistoryViewModel; }
            set
            {
                if (ContentControl.DataContext != value)
                {
                    if (ViewModel != null)
                        ViewModel.Builds.CollectionChanged -= Builds_CollectionChanged;

                    ContentControl.DataContext = value;
                    if (ViewModel != null)
                    {
                        ViewModel.Builds.CollectionChanged += Builds_CollectionChanged;
                        Builds_CollectionChanged(null, null);
                    }
                }
            }
        }

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public BuildHistoryWindow() :
            base(null)
        {
            // Set the window title reading it from the resources.
            this.Caption = "Build History";
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
            base.Content = new BuildHistoryControl();

            ViewModel = null;
        }

        private void Builds_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ViewModel != null)
                Caption = String.Format("Build History ({0})", ViewModel.Builds.Count);
        }
    }
}
