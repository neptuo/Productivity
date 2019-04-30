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
        #region Infrastructure

        private static readonly Dictionary<object, object> storage = new Dictionary<object, object>();

        private static T Get<T>(Func<T> factory)
        {
            object key = factory.Method.GetHashCode();
            if (!storage.TryGetValue(key, out object instance))
                storage[key] = instance = factory();

            return (T)instance;
        }

        #endregion

        public static IFileService FileService => Get(() => new MockFileService());

        public static MainViewModel Main => Get(() => new MainViewModel(FileService)
        {
            Path = "Neptuo.Productivity.AddNewItem/VisualStudio/Commands/",
            Name = "../Views/NewItemView.xaml"
        });
    }
}
