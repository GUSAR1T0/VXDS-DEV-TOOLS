using System;

namespace VXDesign.Store.DevTools.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class HttpQueryParameterAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal class HttpBodyParameterAttribute : Attribute
    {
    }
}