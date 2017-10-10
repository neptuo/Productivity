using Neptuo.Productivity.VisualStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Views.DesignData
{
    public class ViewModelLocator
    {
        private static MainViewModel main;

        public static MainViewModel Main
        {
            get
            {
                if (main == null)
                {
                    main = new MainViewModel();
                    main.Path = "Neptuo.Productivity.AddNewItem/VisualStudio/Commands/";
                    main.Name = "../Views/NewItemView.xaml";
                }

                return main;
            }
        }
    }
}
