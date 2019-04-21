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

            Append(result, ref length, 60 * 1000, "m", 2);
            Append(result, ref length, 1000, "s", 2);
            AppendFormat(result, length, "ms", 3);
            return result.ToString();
        }

        private void Append(StringBuilder result, ref long length, long divisor, string suffix, int leadingZeros)
        {
            if (length > divisor || result.Length > 0)
            {
                AppendFormat(result, Math.Round(length / (double)divisor), suffix, leadingZeros);
                length %= divisor;
            }
        }

        private void AppendFormat(StringBuilder result, double value, string suffix, int leadingZeros)
        {
            if (result.Length > 0)
                result.Append(value.ToString(new String('0', leadingZeros)));
            else
                result.Append(value.ToString());

            result.Append(suffix);
        }
    }
}
