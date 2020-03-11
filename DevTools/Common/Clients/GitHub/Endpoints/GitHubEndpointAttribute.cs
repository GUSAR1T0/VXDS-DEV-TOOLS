using System;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Endpoints
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