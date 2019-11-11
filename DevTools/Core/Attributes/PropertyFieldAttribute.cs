using System;

namespace VXDesign.Store.DevTools.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyFieldAttribute : Attribute
    {
        public string Key { get; set; }
    }
}