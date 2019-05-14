using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Neptuo.Productivity.VisualStudio.Views
{
    public partial class AddNewItemView : UserControl
    {
        public MainViewModel ViewModel => (MainViewModel)DataContext; 

        public AddNewItemView()
        {
            InitializeComponent();
            SetResourceReference(BackgroundProperty, VsBrushes.ToolWindowBackgroundKey);
        }

        public void AutoFocus() => tbxName.Focus();
    }
}
