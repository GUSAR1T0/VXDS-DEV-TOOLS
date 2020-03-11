using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Operation;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IOperationStore
    {
        Task<(long total, IEnumerable<OperationEntity> operations)> Get(IOperation operation, OperationPagingRequest request);
    }

    public class OperationStore : BaseDataStore, IOperationStore
    {
        public async Task<(long total, IEnumerable<OperationEntity> operations)> Get(IOperation operation, OperationPagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [base].[Operation] bo
                LEFT JOIN [authentication].[User] au ON au.[Id] = bo.[UserId]
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    bo.[Id],
                    bo.[Scope],
                    bo.[ContextName],
                    bo.[UserId],
                    au.[Color],
                    au.[FirstName],
                    au.[LastName],
                    bo.[IsSystemAction],
                    bo.[IsSuccessful],
                    bo.[StartTime],
                    bo.[StopTime]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY bo.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var operations = await reader.ReadAsync<OperationEntity>();
            return (total, operations);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(OperationPagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            if (filter.Ids?.Any() == true)
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("bo.[Id] IN @Ids");
            }

            if (filter.Scopes?.Any() == true)
            {
                @params.Add("Scopes", filter.Scopes.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Scopes sc ON bo.[Scope] LIKE sc.[Value]");
            }

            if (filter.ContextNames?.Any() == true)
            {
                @params.Add("ContextNames", filter.ContextNames.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @ContextNames cn ON bo.[ContextName] LIKE cn.[Value]");
            }

            if (filter.UserIds?.Any() == true)
            {
                var userIds = new List<int>(filter.UserIds);
                var userIdsFilter = "bo.[UserId] IN @UserIds";

                if (filter.UserIds.Contains(0))
                {
                    userIds.Remove(0);
                    userIdsFilter = $"(bo.[UserId] IS NULL AND bo.[IsSystemAction] = 0 OR {userIdsFilter})";
                }

                @params.Add("UserIds", userIds);
                filters.Add(userIdsFilter);
            }

            if (filter.IsSystemAction != null)
            {
                @params.Add("IsSystemAction", filter.IsSystemAction);
                filters.Add("bo.[IsSystemAction] = @IsSystemAction");
            }

            if (filter.IsSuccessful != null)
            {
                @params.Add("IsSuccessful", filter.IsSuccessful);
                filters.Add("bo.[IsSuccessful] = @IsSuccessful");
            }

            if (filter.StartTimeRange?.HasRange == true)
            {
                @params.Add("StartTimeMin", filter.StartTimeRange.Min);
                @params.Add("StartTimeMax", filter.StartTimeRange.Max);
                filters.Add("bo.[StartTime] BETWEEN @StartTimeMin AND @StartTimeMax");
            }

            if (filter.StopTimeRange?.HasRange == true)
            {
                @params.Add("StopTimeMin", filter.StopTimeRange.Min);
                @params.Add("StopTimeMax", filter.StopTimeRange.Max);
                filters.Add("bo.[StopTime] BETWEEN @StopTimeMin AND @StopTimeMax");
            }

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }
    }
}