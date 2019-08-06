namespace VXDesign.Store.DevTools.Common.Extensions.Base
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;
            return str.Length >= 1 ? char.ToLowerInvariant(str[0]) + str.Substring(1) : char.ToLowerInvariant(str[0]).ToString();
        }
    }
}