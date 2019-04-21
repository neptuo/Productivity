using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Neptuo.Productivity.UI.Views
{
    public partial class QuickView : UserControl
    {
        public QuickView()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                SetResourceReference(BackgroundProperty, VsBrushes.WindowKey);
                SetResourceReference(ForegroundProperty, VsBrushes.WindowTextKey);
            }
        }
    }
}
