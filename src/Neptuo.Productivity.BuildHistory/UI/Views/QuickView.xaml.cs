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

namespace Neptuo.Productivity.UI.Views
{
    /// <summary>
    /// Interaction logic for BuildHistoryControl.xaml
    /// </summary>
    public partial class QuickView : UserControl
    {
        public QuickView()
        {
            InitializeComponent();

            //DataContext = new BuildHistoryViewModel(
            //    new ObservableCollection<BuildModel>(
            //        new List<BuildModel>()
            //        {
            //            new BuildModel(BuildScope.Project, BuildAction.Build, DateTime.Now),
            //            new BuildModel(BuildScope.Project, BuildAction.Build, DateTime.Now),
            //            new BuildModel(BuildScope.Project, BuildAction.Build, DateTime.Now)
            //        }
            //    )
            //);
        }
    }
}
