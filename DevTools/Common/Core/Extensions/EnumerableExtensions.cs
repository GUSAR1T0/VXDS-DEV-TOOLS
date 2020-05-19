using System.Collections.Generic;
using System.Linq;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Combine<T>(this T mandatoryField, params T[] additionalFields)
        {
            var list = new List<T> { mandatoryField };
            if (additionalFields?.Any() == true)
            {
                list.AddRange(additionalFields);
            }

            return list;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();
    }
}