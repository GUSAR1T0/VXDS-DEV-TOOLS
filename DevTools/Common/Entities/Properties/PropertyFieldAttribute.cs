using System;

namespace VXDesign.Store.DevTools.Common.Entities.Properties
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyFieldAttribute : Attribute
    {
        public string Key { get; set; }
    }
}