using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.Log;

namespace VXDesign.Store.DevTools.Core.Storage.DataStores
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
            var (@params, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, filters)};
                {string.Format(selectBase, selectEntity, filters)}
                ORDER BY bo.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var operations = await reader.ReadAsync<OperationEntity>();
            return (total, operations);
        }

        private static (DynamicParameters, string) HandleGetRequest(OperationPagingFilter filter)
        {
            var @params = new DynamicParameters();
            var filters = new List<string>();

            if (filter.Ids?.Any() == true)
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("bo.[Id] IN @Ids");
            }

            if (filter.Scopes?.Any() == true)
            {
                @params.Add("Scopes", filter.Scopes);
                filters.Add("bo.[Scope] IN @Scopes");
            }

            if (filter.ContextNames?.Any() == true)
            {
                @params.Add("ContextNames", filter.ContextNames);
                filters.Add("bo.[ContextName] IN @ContextNames");
            }

            if (filter.UserIds?.Any() == true)
            {
                @params.Add("UserIds", filter.UserIds);
                filters.Add("bo.[UserId] IN @UserIds");
            }

            if (filter.IsSystemAction != null)
            {
                filters.Add($"bo.[IsSystemAction] = {(filter.IsSystemAction == true ? 1 : 0)}");
            }

            if (filter.IsSuccessful != null)
            {
                filters.Add($"bo.[IsSuccessful] = {(filter.IsSuccessful == true ? 1 : 0)}");
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

            return (@params, filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }
    }
}