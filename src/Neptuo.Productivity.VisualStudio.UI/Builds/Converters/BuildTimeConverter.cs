using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Neptuo.Productivity.VisualStudio.UI.Builds.Converters
{
    public class BuildTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long? lengthValue = value as long?;
            if (lengthValue == null)
                return "Building...";

            long length = lengthValue.Value;
            StringBuilder result = new StringBuilder();

            if (length > 60 * 1000)
            {
                result.AppendFormat("{0}m ", Math.Round(length / (60 * 1000D)));
                length = length % (60 * 1000);
            }

            if(length > 1000)
            {
                result.AppendFormat("{0}s ", Math.Round(length / 1000D));
                length = length % 1000;
            }

            if (length != 0)
                result.AppendFormat("{0}ms", length);

            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
