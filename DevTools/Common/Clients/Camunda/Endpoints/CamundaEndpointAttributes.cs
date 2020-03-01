using System;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints
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