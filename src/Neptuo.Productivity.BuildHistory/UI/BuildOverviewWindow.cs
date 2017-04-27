using Microsoft.VisualStudio.Shell;
using Neptuo.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows;

namespace Neptuo.Productivity.VisualStudio.UI
{
    [Guid("04576E43-F435-4C2B-A2EE-B56B9C3941DA")]
    public class BuildOverviewWindow : ToolWindowPane
    {
        //protected QuickView ContentControl
        //{
        //    get { return (QuickView)Content; }
        //    set { Content = value; }
        //}

        //public QuickMainViewModel ViewModel
        //{
        //    get { return ContentControl.DataContext as QuickMainViewModel; }
        //    set
        //    {
        //        if (ContentControl.DataContext != value)
        //        {
        //            if (ViewModel != null)
        //                ViewModel.TitleChanged -= ViewModel_TitleChanged;

        //            ContentControl.DataContext = value;
        //            if (ViewModel != null)
        //            {
        //                ViewModel.TitleChanged += ViewModel_TitleChanged;
        //                ViewModel_TitleChanged(ViewModel, ViewModel.Title);
        //            }
        //        }
        //    }
        //}

        public BuildOverviewWindow()
            : base(null)
        {
            Caption = "Build History";
            //BitmapResourceID = 301;
            //BitmapIndex = 1;

            Content = new TextBlock()
            {
                Text = "Comming Soon...",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            //Content = new QuickView();

            //ViewModel = new QuickMainViewModel(ServiceFactory.EventHandlers, ServiceFactory.Configuration);
            //ViewModel.TitleChanged += ViewModel_TitleChanged;
        }

        //private void ViewModel_TitleChanged(QuickMainViewModel sender, string title)
        //{
        //    Caption = title;
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);

        //    if (disposing && ViewModel != null)
        //        ViewModel.TitleChanged -= ViewModel_TitleChanged;
        //}
    }
}
