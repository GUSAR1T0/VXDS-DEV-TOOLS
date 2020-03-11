using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Properties;

namespace VXDesign.Store.DevTools.Common.Core.Utils
{
    public static class PropertiesUtils
    {
        public static T Create<T>(IConfiguration configuration) where T : IPropertiesMarker, new()
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
                var key = GetConfigurationKey(attribute, property, prefix);
                var value = typeof(IPropertiesMarker).IsAssignableFrom(property.PropertyType) ? Create(configuration, property.PropertyType, key) : configuration[key];
                property.SetPropertyValue(properties, value);
            }

            return properties;
        }

        private static string GetConfigurationKey(PropertyFieldAttribute attribute, PropertyInfo property, string prefix = null)
        {
            return (!string.IsNullOrWhiteSpace(prefix) ? prefix + ':' : "") + (attribute?.Key ?? property.Name);
        }

        private static PropertyFieldAttribute GetPropertyFieldAttribute(Type type, string propertyName)
        {
            return type.GetProperty(propertyName).GetCustomAttributes<PropertyFieldAttribute>(false).FirstOrDefault();
        }
    }
}