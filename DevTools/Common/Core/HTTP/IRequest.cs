using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.File;

namespace VXDesign.Store.DevTools.Common.Core.HTTP
{
    public interface IRequest
    {
        Dictionary<string, string> Path { get; }
        Dictionary<string, string> Query { get; }
        string Body { get; }
        IReadOnlyList<LocalFile> Resources { get; }
    }
}