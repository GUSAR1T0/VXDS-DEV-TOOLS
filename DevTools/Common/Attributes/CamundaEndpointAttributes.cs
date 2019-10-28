using System;
using VXDesign.Store.DevTools.Common.Enums.Camunda;
using VXDesign.Store.DevTools.Common.Enums.HTTP;

namespace VXDesign.Store.DevTools.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class CamundaActionAttribute : Attribute
    {
        internal CamundaCategory CamundaCategory { get; }
        internal string Description { get; }
        internal HttpMethod Method { get; }
        internal string Path { get; }

        internal CamundaActionAttribute(CamundaCategory camundaCategory, string description, HttpMethod method, string path = "")
        {
            CamundaCategory = camundaCategory;
            Description = description;
            Method = method;
            Path = path;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal class CamundaCategoryAttribute : Attribute
    {
        internal string Name { get; }
        internal string Root { get; }

        internal CamundaCategoryAttribute(string name, string root)
        {
            Name = name;
            Root = root;
        }
    }
}