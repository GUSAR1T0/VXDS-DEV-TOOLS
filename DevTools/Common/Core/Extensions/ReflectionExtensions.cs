using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class ReflectionExtensions
    {
        public static void SetPropertyValue<T>(this PropertyInfo property, T target, object value)
        {
            static object GetSafeValue(object possible, Type objectType)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(possible?.ToString()))
                    {
                        return null;
                    }

                    if (possible is IEnumerable enumerable)
                    {
                        return enumerable;
                    }

                    if (objectType.IsEnum)
                    {
                        return Enum.Parse(objectType, possible.ToString()!);
                    }

                    var underlyingType = Nullable.GetUnderlyingType(objectType);
                    if (underlyingType != null && underlyingType.IsEnum)
                    {
                        return Enum.Parse(objectType, possible.ToString()!);
                    }

                    return Convert.ChangeType(possible, objectType, CultureInfo.InvariantCulture);
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

        public static Type GetUnderlyingTypeFromPossibleEnumType(this Type type)
        {
            if (type.IsEnum)
            {
                return Enum.GetUnderlyingType(type);
            }

            var underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType != null && underlyingType.IsEnum ? Enum.GetUnderlyingType(underlyingType) : null;
        }
    }
}