using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using VXDesign.Store.DevTools.Common.Core.HTTP;

namespace VXDesign.Store.DevTools.Common.Clients.GitHub.Base
{
    public interface IGitHubResponse : IResponse
    {
    }

    public class IntermediateGitHubResponse<TObject> : IGitHubResponse
    {
        public int Status { get; set; }
        public string Output { get; set; }
        public string Reason { get; set; }
        public TObject Response { get; set; }
    }

    public abstract class GitHubEmptyResponse : IntermediateGitHubResponse<EmptyResult>
    {
    }

    public abstract class GitHubSingleResponse<TEntity> : IntermediateGitHubResponse<TEntity> where TEntity : IGitHubEntity
    {
    }

    public abstract class GitHubMultipleResponse<TEntity> : IntermediateGitHubResponse<List<TEntity>> where TEntity : IGitHubEntity
    {
    }

    public abstract class GitHubDynamicResponse : IntermediateGitHubResponse<dynamic>
    {
        public IReadOnlyDictionary<string, TValue> ToDictionary<TValue>() => ((JObject) Response).ToObject<Dictionary<string, TValue>>();
    }
}