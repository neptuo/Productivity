using Neptuo.Models.Keys;
using Neptuo.Exceptions.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public static class _EnsureConditionExtensions
    {
        public static void NotNullOrEmpty(this EnsureConditionHelper condition, IKey key, string argumentName)
        {
            Ensure.NotNull(condition, "condition");
            Ensure.NotNull(key, argumentName);

            if (key.IsEmpty)
                throw Ensure.Exception.ArgumentOutOfRange(argumentName, "Key can't be empty.");
        }
    }
}
