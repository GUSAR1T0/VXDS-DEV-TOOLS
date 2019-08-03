using System;
using VXDesign.Store.DevTools.Common.Entities.Enums;

namespace VXDesign.Store.DevTools.Common.Attributes.Camunda
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class CamundaActionAttribute : Attribute
    {
        internal CamundaCategory CamundaCategory { get; }
        internal string Name { get; }
        internal HttpMethod Method { get; }
        internal string Path { get; }

        internal CamundaActionAttribute(CamundaCategory camundaCategory, string name, HttpMethod method, string path = "")
        {
            CamundaCategory = camundaCategory;
            Name = name;
            Method = method;
            Path = path;
        }
    }
}