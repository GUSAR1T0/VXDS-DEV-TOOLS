using System;
using System.Globalization;
using System.Reflection;

namespace VXDesign.Store.DevTools.Common.Extensions.Base
{
    public static class PropertyInfoExtensions
    {
        public static void SetPropertyValue<T>(this PropertyInfo property, T target, object value)
        {
            object GetSafeValue(object possible, Type objectType)
            {
                try
                {
                    return string.IsNullOrWhiteSpace(possible?.ToString()) ? null : Convert.ChangeType(possible, objectType, CultureInfo.InvariantCulture);
                }
                catch
                {
                    return null;
                }
            }

            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            var safeValue = GetSafeValue(value, type);
            if (safeValue != null)
            {
                property.SetValue(target, safeValue, null);
            }
        }
    }
}