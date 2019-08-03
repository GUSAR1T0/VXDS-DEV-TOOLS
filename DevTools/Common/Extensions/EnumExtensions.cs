using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace VXDesign.Store.DevTools.Common.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();

        public static IEnumerable<T> GetAttributesOfType<T>(this Enum value) where T : Attribute => value.GetType().GetMember(value.ToString())[0].GetCustomAttributes<T>(false).ToList();

        public static T GetAttributeOfType<T>(this Enum value) where T : Attribute => GetAttributesOfType<T>(value).FirstOrDefault();

        public static string GetDescription(this Enum value) => value.GetAttributeOfType<DescriptionAttribute>()?.Description;
    }
}