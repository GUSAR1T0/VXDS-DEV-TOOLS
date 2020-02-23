using System.Collections.Generic;
using VXDesign.Store.DevTools.Core.Entities.HTTP;

namespace VXDesign.Store.DevTools.Core.Entities.GitHub.Base
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
}