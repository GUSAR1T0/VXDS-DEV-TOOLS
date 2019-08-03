using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Utils.Properties
{
    internal static class PropertiesCreator
    {
        internal static T Create<T>(IConfiguration configuration) where T : PropertiesMarker, new()
        {
            var properties = new T();
            return FillProperties(configuration, properties);
        }

        private static object Create(IConfiguration configuration, Type type, string prefix = null)
        {
            var properties = type.GetConstructor(new Type[0]).Invoke(new object[0]);
            return FillProperties(configuration, properties, prefix);
        }

        private static T FillProperties<T>(IConfiguration configuration, T properties, string prefix = null)
        {
            var type = properties.GetType();
            foreach (var property in type.GetProperties().Where(property => property.GetCustomAttributes<PropertyFieldAttribute>().Any()))
            {
                var attribute = GetPropertyFieldAttribute(type, property.Name);
                var key = GetConfigurationKey(attribute, prefix);
                var value = property.PropertyType.IsSubclassOf(typeof(PropertiesMarker)) ? Create(configuration, property.PropertyType, key) : configuration[key];
                SetPropertyValue(property, attribute, properties, value);
            }

            return properties;
        }

        private static string GetConfigurationKey(PropertyFieldAttribute attribute, string prefix = null) => (!string.IsNullOrWhiteSpace(prefix) ? prefix + '.' : "") + attribute?.Key;

        private static PropertyFieldAttribute GetPropertyFieldAttribute(Type type, string propertyName)
        {
            return type.GetProperty(propertyName).GetCustomAttributes<PropertyFieldAttribute>(false).FirstOrDefault();
        }

        private static void SetPropertyValue<T>(PropertyInfo property, PropertyFieldAttribute attribute, T portalModuleProperties, object value)
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
            var safeValue = GetSafeValue(value, type) ?? GetSafeValue(attribute.Default, type);
            property.SetValue(portalModuleProperties, safeValue, null);
        }
    }
}