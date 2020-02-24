using System.Collections.Generic;
using VXDesign.Store.DevTools.Core.Entities.Storage.GitHub;
using VXDesign.Store.DevTools.Core.Entities.Storage.SSP;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.Project
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public long? GitHubRepoId { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProjectWithRepositoryInfo : IPagingResponseItemEntity
    {
        public ProjectEntity Project { get; set; }
        public GitHubRepositoryEntity GitHubRepository { get; set; }
    }

    public class ProjectPagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<string> Aliases { get; set; }
        public IEnumerable<int> GitHubRepoIds { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ProjectPagingRequest : ServerSidePagingRequest<ProjectPagingFilter>
    {
    }

    public class ProjectPagingResponse : ServerSidePagingResponse<ProjectWithRepositoryInfo>
    {
    }
}