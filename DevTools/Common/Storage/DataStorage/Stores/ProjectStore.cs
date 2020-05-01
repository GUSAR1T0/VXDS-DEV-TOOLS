using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.Project;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IProjectStore
    {
        Task<(long total, IEnumerable<ProjectListItemEntity> projects)> GetProjects(IOperation operation, ProjectPagingRequest request);
        Task<IEnumerable<ProjectSearchEntity>> SearchProjectsByPattern(IOperation operation, string pattern);
        Task<ProjectProfileEntity> Get(IOperation operation, int id);
        Task<IEnumerable<byte>> CheckFieldsForProjectCreation(IOperation operation, string name, string alias, long? gitHubRepoId, int? id = null);
        Task<bool> IsProjectExist(IOperation operation, int id);
        Task<int> AddProject(IOperation operation, ProjectProfileEntity entity);
        Task UpdateProject(IOperation operation, ProjectProfileEntity entity);
        Task DeleteProject(IOperation operation, int id);
    }

    public class ProjectStore : BaseDataStore, IProjectStore
    {
        public async Task<(long total, IEnumerable<ProjectListItemEntity> projects)> GetProjects(IOperation operation, ProjectPagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [portal].[Project] pp
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    pp.[Id],
                    pp.[Name],
                    pp.[Alias],
                    pp.[Description],
                    pp.[GitHubRepoId],
                    pp.[IsActive]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY pp.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var projects = await reader.ReadAsync<ProjectListItemEntity>();
            return (total, projects);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(ProjectPagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            if (!filter.Ids.IsNullOrEmpty())
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("pp.[Id] IN @Ids");
            }

            if (!filter.Names.IsNullOrEmpty())
            {
                @params.Add("Names", filter.Names.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Names name ON pp.[Name] LIKE name.[Value]");
            }

            if (!filter.Aliases.IsNullOrEmpty())
            {
                @params.Add("Aliases", filter.Aliases.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Aliases alias ON pp.[Alias] LIKE alias.[Value]");
            }

            if (!filter.GitHubRepoIds.IsNullOrEmpty())
            {
                @params.Add("GitHubRepoIds", filter.GitHubRepoIds);
                filters.Add("pp.[GitHubRepoId] IN @GitHubRepoIds");
            }

            if (filter.IsActive != null)
            {
                @params.Add("IsActive", filter.IsActive);
                filters.Add("pp.[IsActive] = @IsActive");
            }

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }

        public async Task<IEnumerable<ProjectSearchEntity>> SearchProjectsByPattern(IOperation operation, string pattern)
        {
            return await operation.Connection.QueryAsync<ProjectSearchEntity>(new { Pattern = $"%{pattern}%" }, $@"
                SELECT TOP {FormatPattern.SearchMaxCount}
                    [Id],
                    [Name],
                    [Alias]
                FROM [portal].[Project]
                WHERE [Name] LIKE @Pattern OR [Alias] LIKE @Pattern;
            ");
        }

        public async Task<ProjectProfileEntity> Get(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<ProjectProfileEntity>(new { Id = id }, @"
                SELECT
                    [Id],
                    [Name],
                    [Alias],
                    [Description],
                    [GitHubRepoId],
                    [IsActive]
                FROM [portal].[Project]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<IEnumerable<byte>> CheckFieldsForProjectCreation(IOperation operation, string name, string alias, long? gitHubRepoId, int? id = null)
        {
            return await operation.Connection.QueryAsync<byte>(new
            {
                Name = name,
                Alias = alias,
                GitHubRepoId = gitHubRepoId,
                Id = id
            }, @"
                DECLARE @Errors TABLE ([Code] TINYINT);

                INSERT INTO @Errors
                SELECT TOP (1) 1
                FROM [portal].[Project]
                WHERE [Name] = @Name AND (@Id IS NULL OR [Id] <> @Id);

                INSERT INTO @Errors
                SELECT TOP (1) 2
                FROM [portal].[Project]
                WHERE [Alias] = @Alias AND (@Id IS NULL OR [Id] <> @Id);

                INSERT INTO @Errors
                SELECT TOP (1) 3
                FROM [portal].[Project]
                WHERE [GitHubRepoId] = @GitHubRepoId AND (@Id IS NULL OR [Id] <> @Id);

                SELECT [Code] FROM @Errors;
            ");
        }

        public async Task<bool> IsProjectExist(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = id }, @"
                SELECT TOP (1) 1
                FROM [portal].[Project]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<int> AddProject(IOperation operation, ProjectProfileEntity entity)
        {
            return await operation.Connection.QueryFirstOrDefaultAsync<int>(entity, @"
                INSERT INTO [portal].[Project] ([Name], [Alias], [Description], [GitHubRepoId], [IsActive])
                OUTPUT INSERTED.[Id]
                VALUES (@Name, @Alias, @Description, @GitHubRepoId, @IsActive);
            ");
        }

        public async Task UpdateProject(IOperation operation, ProjectProfileEntity entity)
        {
            await operation.Connection.ExecuteAsync(entity, @"
                UPDATE [portal].[Project]
                SET
                    [Name] = @Name,
                    [Alias] = @Alias,
                    [Description] = @Description,
                    [GitHubRepoId] = @GitHubRepoId,
                    [IsActive] = @IsActive
                WHERE [Id] = @Id;
            ");
        }

        public async Task DeleteProject(IOperation operation, int id)
        {
            await operation.Connection.ExecuteAsync(new { Id = id }, @"
                DELETE FROM [portal].[Project]
                WHERE [Id] = @Id;
            ");
        }
    }
}