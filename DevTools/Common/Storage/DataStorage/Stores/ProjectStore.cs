using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Project;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IProjectStore
    {
        Task<(long total, IEnumerable<ProjectEntity> projects)> Get(IOperation operation, ProjectPagingRequest request);
    }

    public class ProjectStore : BaseDataStore, IProjectStore
    {
        public async Task<(long total, IEnumerable<ProjectEntity> projects)> Get(IOperation operation, ProjectPagingRequest request)
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
            var projects = await reader.ReadAsync<ProjectEntity>();
            return (total, projects);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(ProjectPagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            if (filter.Ids?.Any() == true)
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("pp.[Id] IN @Ids");
            }

            if (filter.Names?.Any() == true)
            {
                @params.Add("Names", filter.Names.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Names name ON pp.[Name] LIKE name.[Value]");
            }

            if (filter.Aliases?.Any() == true)
            {
                @params.Add("Aliases", filter.Aliases.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Aliases alias ON pp.[Alias] LIKE alias.[Value]");
            }

            if (filter.GitHubRepoIds?.Any() == true)
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
    }
}