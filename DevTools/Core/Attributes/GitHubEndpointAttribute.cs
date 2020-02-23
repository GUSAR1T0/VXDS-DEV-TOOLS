using System;
using VXDesign.Store.DevTools.Core.Enums.HTTP;

namespace VXDesign.Store.DevTools.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class GitHubEndpointAttribute : Attribute
    {
        internal string Description { get; }
        internal HttpMethod Method { get; }
        internal string Path { get; }

        internal GitHubEndpointAttribute(string description, HttpMethod method, string path)
        {
            Description = description;
            Method = method;
            Path = path;
        }
    }
}