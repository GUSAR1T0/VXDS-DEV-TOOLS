using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Entities;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Models.Repositories;
using VXDesign.Store.DevTools.Common.Core.Entities.GitHub;
using VXDesign.Store.DevTools.Common.Core.Entities.Project;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Services
{
    public interface IProjectService
    {
        Task<ProjectPagingResponse> GetItems(IOperation operation, ProjectPagingRequest request);
        Task<IEnumerable<GitHubRepositoryShortEntity>> SearchGitHubRepositoriesByPattern(IOperation operation, string pattern);
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectStore projectStore;
        private readonly IPortalSettingsService portalSettingsService;
        private readonly IMemoryCacheService memoryCacheService;

        private const string GitHubUserRepositoriesKey = "GitHubUserRepositories";

        public ProjectService(IProjectStore projectStore, IPortalSettingsService portalSettingsService, IMemoryCacheService memoryCacheService)
        {
            this.projectStore = projectStore;
            this.portalSettingsService = portalSettingsService;
            this.memoryCacheService = memoryCacheService;
        }

        public async Task<ProjectPagingResponse> GetItems(IOperation operation, ProjectPagingRequest request)
        {
            var (total, projects) = await projectStore.Get(operation, request);
            var repositories = total == 0 ? new List<RepositoryEntity>() : await GetGitHubUserRepositoriesFromCache(operation);
            return new ProjectPagingResponse
            {
                Total = total,
                Items = PreparePagingItems(projects, repositories)
            };
        }

        private async Task<List<RepositoryEntity>> GetGitHubUserRepositoriesFromCache(IOperation operation)
        {
            return await memoryCacheService.Get(GitHubUserRepositoriesKey, async () =>
            {
                var gitHubClient = await portalSettingsService.GetGitHubClient(operation);
                var response = await new Repositories.GetUserRepositoriesRequest().SendRequest(operation, gitHubClient);
                return response.IsWithoutErrors() ? response.Response : throw CommonExceptions.UserRepositoriesCouldNotBeLoaded(operation, response.Output);
            });
        }

        private static IEnumerable<ProjectWithRepositoryInfo> PreparePagingItems(IEnumerable<ProjectEntity> projects, IReadOnlyCollection<RepositoryEntity> repositories)
        {
            return projects.Select(project =>
            {
                var gitHubRepository = repositories.FirstOrDefault(repository => repository.Id == project.GitHubRepoId);
                return new ProjectWithRepositoryInfo
                {
                    Project = project,
                    GitHubRepository = gitHubRepository != null
                        ? new GitHubRepositoryEntity
                        {
                            Id = gitHubRepository.Id,
                            FullName = gitHubRepository.FullName,
                            Private = gitHubRepository.Private,
                            Owner = new GitHubUserEntity
                            {
                                IsValid = true,
                                Login = gitHubRepository.Owner.Login,
                                Name = gitHubRepository.Owner.Name,
                                AvatarUrl = gitHubRepository.Owner.AvatarUrl,
                                ProfileUrl = gitHubRepository.Owner.HtmlUrl
                            },
                            RepositoryUrl = gitHubRepository.HtmlUrl
                        }
                        : null
                };
            });
        }

        public async Task<IEnumerable<GitHubRepositoryShortEntity>> SearchGitHubRepositoriesByPattern(IOperation operation, string pattern)
        {
            var repositories = await GetGitHubUserRepositoriesFromCache(operation);
            return repositories.Where(repository => repository.FullName.Contains(pattern, StringComparison.InvariantCultureIgnoreCase)).Select(repository => new GitHubRepositoryShortEntity
            {
                Id = repository.Id,
                FullName = repository.FullName
            });
        }
    }
}