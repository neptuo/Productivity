using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.VisualStudio.Views.DesignData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Neptuo.Productivity.VisualStudio.Views
{
    [Guid("088f3f0e-a9e1-4e31-b77b-69e6d0fa5294")]
    public class AddNewItemWindow : ToolWindowPane
    {
        public AddNewItemWindow()
        {
            Caption = "Add new item...";
            Content = new AddNewItemView()
            {
                DataContext = ViewModelLocator.Main
            };
        }
    }
}
