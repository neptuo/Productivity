using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.UI.ViewModels
{
    public class BuildTimeFormatter
    {
        public string Format(long elapsedMilliseconds)
        {
            long length = elapsedMilliseconds;
            StringBuilder result = new StringBuilder();

            if (length > 60 * 1000)
            {
                result.AppendFormat("{0}m ", Math.Round(length / (60 * 1000D)));
                length = length % (60 * 1000);
            }

            if (length > 1000)
            {
                result.AppendFormat("{0}s ", Math.Round(length / 1000D));
                length = length % 1000;
            }

            result.AppendFormat("{0}ms", length);
            return result.ToString();
        }
    }
}
