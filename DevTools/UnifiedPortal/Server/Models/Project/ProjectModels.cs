using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Core.Entities.Storage.Project;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.GitHub;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Project
{
    public class ProjectModel : PagingResponseItemModel, IPagingResponseItemModel<ProjectModel, ProjectWithRepositoryInfo>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public GitHubRepositoryModel GitHubRepository { get; set; }

        public ProjectModel ToModel(ProjectWithRepositoryInfo entity)
        {
            Id = entity.Project.Id;
            Name = entity.Project.Name;
            Alias = entity.Project.Alias;
            Description = entity.Project.Description;
            IsActive = entity.Project.IsActive;
            GitHubRepository = entity.GitHubRepository.ToModel();
            return this;
        }
    }

    public class ProjectPagingFilterModel : PagingFilterModel, IPagingFilterModel<ProjectPagingFilter>
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<string> Aliases { get; set; }
        public IEnumerable<int> GitHubRepoIds { get; set; }
        public bool? IsActive { get; set; }

        public ProjectPagingFilter ToEntity() => new ProjectPagingFilter
        {
            Ids = Ids,
            Names = Names,
            Aliases = Aliases,
            GitHubRepoIds = GitHubRepoIds,
            IsActive = IsActive
        };
    }

    public class ProjectPagingRequestModel : ServerSidePagingRequestModel<ProjectPagingFilterModel, ProjectPagingRequest>
    {
        public override ProjectPagingRequest ToEntity() => new ProjectPagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class ProjectPagingResponseModel : ServerSidePagingResponseModel<ProjectModel, ProjectPagingResponseModel, ProjectPagingResponse>
    {
        public override ProjectPagingResponseModel ToModel(ProjectPagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new ProjectModel().ToModel(item));
            return this;
        }
    }
}