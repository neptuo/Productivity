using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Neptuo.Productivity.VisualStudio.UI.Builds.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? dateTime = value as DateTime?;
            if (dateTime != null)
            {
                if (dateTime.Value.Date == DateTime.Today)
                    return dateTime.Value.ToShortTimeString();

                return dateTime.Value.ToString();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
