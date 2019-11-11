using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Core.Entities.HTTP
{
    public interface IRequest
    {
        Dictionary<string, string> Path { get; }
        Dictionary<string, string> Query { get; }
        string Body { get; }
    }
}