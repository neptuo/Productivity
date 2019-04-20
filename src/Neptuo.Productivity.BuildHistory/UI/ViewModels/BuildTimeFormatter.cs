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
                AppendFormat(result, Math.Round(length / (60 * 1000D)), "m");
                length = length % (60 * 1000);
            }

            if (length > 1000)
            {
                AppendFormat(result, Math.Round(length / 1000D), "s");
                length = length % 1000;
            }

            AppendFormat(result, length, "ms");
            return result.ToString();
        }

        private void AppendFormat(StringBuilder result, double value, string suffix)
        {
            if (result.Length > 0)
                result.Append(value.ToString("000"));
            else
                result.Append(value.ToString());

            result.Append(suffix);
        }
    }
}
