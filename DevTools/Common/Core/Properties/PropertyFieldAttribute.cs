using System;

namespace VXDesign.Store.DevTools.Common.Core.Properties
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyFieldAttribute : Attribute
    {
        public string Key { get; set; }
    }
}