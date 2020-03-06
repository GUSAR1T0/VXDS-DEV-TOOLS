using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Clients.GitHub;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Entities;
using VXDesign.Store.DevTools.Common.Clients.GitHub.Models.Repositories;
using VXDesign.Store.DevTools.Common.Core.Constants;
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
        Task<ProjectProfileGetEntity> GetProjectProfileById(IOperation operation, int id);
        Task<int> CreateProjectProfile(IOperation operation, ProjectProfileEntity entity);
        Task UpdateProjectProfile(IOperation operation, ProjectProfileEntity entity);
        Task DeleteProjectProfile(IOperation operation, int id);
    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectStore projectStore;
        private readonly IPortalSettingsService portalSettingsService;
        private readonly IMemoryCacheService memoryCacheService;

        public ProjectService(IProjectStore projectStore, IPortalSettingsService portalSettingsService, IMemoryCacheService memoryCacheService)
        {
            this.projectStore = projectStore;
            this.portalSettingsService = portalSettingsService;
            this.memoryCacheService = memoryCacheService;
        }

        public async Task<ProjectPagingResponse> GetItems(IOperation operation, ProjectPagingRequest request)
        {
            var (total, projects) = await projectStore.GetProjects(operation, request);
            var repositories = total == 0 ? new List<RepositoryListItemEntity>() : await GetGitHubUserRepositoriesFromCache(operation);
            return new ProjectPagingResponse
            {
                Total = total,
                Items = PreparePagingItems(projects, repositories)
            };
        }

        private async Task<List<RepositoryListItemEntity>> GetGitHubUserRepositoriesFromCache(IOperation operation)
        {
            return await memoryCacheService.Get(MemoryCacheKey.GitHubUserRepositories, async () =>
            {
                IGitHubClientService gitHubClient;
                try
                {
                    gitHubClient = await portalSettingsService.GetGitHubClient(operation);
                }
                catch (NotFoundException)
                {
                    return new List<RepositoryListItemEntity>();
                }

                var response = await new Repositories.GetUserRepositoriesRequest().SendRequest(operation, gitHubClient);
                return response.IsWithoutErrors() ? response.Response : throw CommonExceptions.UserRepositoriesCouldNotBeLoaded(operation, response.Output);
            }, TimeSpan.FromMinutes(5));
        }

        private static IEnumerable<ProjectWithRepositoryInfo> PreparePagingItems(IEnumerable<ProjectListItemEntity> projects, IReadOnlyCollection<RepositoryListItemEntity> repositories)
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
                                Login = gitHubRepository.Owner.Login,
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

        public async Task<ProjectProfileGetEntity> GetProjectProfileById(IOperation operation, int id)
        {
            var project = await projectStore.Get(operation, id);
            var gitHubRepository = (await GetGitHubUserRepositoriesFromCache(operation)).FirstOrDefault(entity => entity.Id == project.GitHubRepoId);
            return new ProjectProfileGetEntity
            {
                Id = project.Id,
                Name = project.Name,
                Alias = project.Alias,
                Description = project.Description,
                GitHubRepoId = project.GitHubRepoId,
                GitHubRepository = gitHubRepository != null
                    ? new GitHubRepositoryFullEntity
                    {
                        Id = gitHubRepository.Id,
                        FullName = gitHubRepository.FullName,
                        Private = gitHubRepository.Private,
                        Owner = new GitHubUserEntity
                        {
                            Login = gitHubRepository.Owner.Login,
                            AvatarUrl = gitHubRepository.Owner.AvatarUrl,
                            ProfileUrl = gitHubRepository.Owner.HtmlUrl
                        },
                        Description = gitHubRepository.Description,
                        RepositoryUrl = gitHubRepository.HtmlUrl,
                        StargazersCount = gitHubRepository.StargazersCount,
                        WatchersCount = gitHubRepository.WatchersCount,
                        SubscribersCount = gitHubRepository.SubscribersCount,
                        OpenIssuesCount = gitHubRepository.OpenIssuesCount,
                        License = gitHubRepository.License != null
                            ? new GitHubLicenseEntity
                            {
                                Name = gitHubRepository.License.Name,
                                Url = gitHubRepository.License.Url
                            }
                            : null
                    }
                    : null,
                IsActive = project.IsActive
            };
        }

        public async Task<int> CreateProjectProfile(IOperation operation, ProjectProfileEntity entity)
        {
            var errorCodes = (await projectStore.CheckFieldsForProjectCreation(operation, entity.Name, entity.Alias, entity.GitHubRepoId)).ToList();
            if (errorCodes.Any())
            {
                throw CommonExceptions.ProjectHasAlreadyExisted(operation, errorCodes, entity.Name, entity.Alias, entity.GitHubRepoId);
            }

            return await projectStore.AddProject(operation, entity);
        }

        public async Task UpdateProjectProfile(IOperation operation, ProjectProfileEntity entity)
        {
            if (!await projectStore.IsProjectExist(operation, entity.Id))
            {
                throw CommonExceptions.ProjectWasNotFound(operation, entity.Id);
            }

            var errorCodes = (await projectStore.CheckFieldsForProjectCreation(operation, entity.Name, entity.Alias, entity.GitHubRepoId, entity.Id)).ToList();
            if (errorCodes.Any())
            {
                throw CommonExceptions.ProjectHasAlreadyExisted(operation, errorCodes, entity.Name, entity.Alias, entity.GitHubRepoId);
            }

            await projectStore.UpdateProject(operation, entity);
        }

        public async Task DeleteProjectProfile(IOperation operation, int id)
        {
            if (!await projectStore.IsProjectExist(operation, id))
            {
                throw CommonExceptions.ProjectWasNotFound(operation, id);
            }

            await projectStore.DeleteProject(operation, id);
        }
    }
}