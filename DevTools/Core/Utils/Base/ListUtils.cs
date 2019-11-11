using System.Collections.Generic;
using System.Linq;

namespace VXDesign.Store.DevTools.Core.Utils.Base
{
    public static class ListUtils
    {
        public static IEnumerable<T> Initialize<T>(T mandatoryField, params T[] additionalFields)
        {
            var list = new List<T> { mandatoryField };
            if (additionalFields?.Any() == true)
            {
                list.AddRange(additionalFields);
            }

            return list;
        }
    }
}