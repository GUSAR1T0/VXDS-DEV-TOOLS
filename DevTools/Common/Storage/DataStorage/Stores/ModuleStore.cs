using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IModuleStore
    {
        Task<(long total, IEnumerable<ModuleEntity> modules)> Get(IOperation operation, ModulePagingRequest request);
    }

    public class ModuleStore : IModuleStore
    {
        public async Task<(long total, IEnumerable<ModuleEntity> modules)> Get(IOperation operation, ModulePagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [portal].[Module] pm
                INNER JOIN [authentication].[User] au ON au.[Id] = pm.[UserId]
                INNER JOIN [portal].[Host] ph ON ph.[Id] = pm.[HostId]
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    pm.[Id],
                    pm.[Name],
                    pm.[Alias],
                    pm.[UserId],
                    au.[Color],
                    au.[FirstName],
                    au.[LastName],
                    pm.[HostId],
                    ph.[Name] AS [HostName],
                    ph.[Domain] AS [HostDomain],
                    ph.[OperationSystemId] AS [HostOperationSystem],
                    pm.[IsActive]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY pm.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var modules = await reader.ReadAsync<ModuleEntity>();
            return (total, modules);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(ModulePagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            if (!filter.Ids.IsNullOrEmpty())
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("pm.[Id] IN @Ids");
            }

            if (!filter.Names.IsNullOrEmpty())
            {
                @params.Add("Names", filter.Names.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Names name ON pm.[Name] LIKE name.[Value]");
            }

            if (!filter.Aliases.IsNullOrEmpty())
            {
                @params.Add("Aliases", filter.Aliases.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Aliases alias ON pm.[Alias] LIKE alias.[Value]");
            }

            if (!filter.HostIds.IsNullOrEmpty())
            {
                @params.Add("HostIds", filter.HostIds);
                filters.Add("pm.[HostId] IN @HostIds");
            }

            if (!filter.UserIds.IsNullOrEmpty())
            {
                @params.Add("UserIds", filter.UserIds);
                filters.Add("pm.[UserId] IN @UserIds");
            }

            if (filter.IsActive != null)
            {
                @params.Add("IsActive", filter.IsActive);
                filters.Add("pm.[IsActive] = @IsActive");
            }

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }
    }
}