using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Notification;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface INotificationStore
    {
        Task<(long total, IEnumerable<NotificationEntity> notifications)> Get(IOperation operation, NotificationPagingRequest request);
    }

    public class NotificationStore : BaseDataStore, INotificationStore
    {
        public async Task<(long total, IEnumerable<NotificationEntity> notifications)> Get(IOperation operation, NotificationPagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [portal].[Notification] pn
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    pn.[Id],
                    pn.[Message],
                    pn.[LevelId] AS [Level],
                    pn.[StartTime],
                    pn.[StopTime]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY pn.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var notifications = await reader.ReadAsync<NotificationEntity>();
            return (total, notifications);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(NotificationPagingFilter filter)
        {
            var @params = new DynamicParameters();
            var filters = new List<string>();

            if (filter.Ids?.Any() == true)
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("pn.[Id] IN @Ids");
            }

            if (filter.Levels?.Any() == true)
            {
                @params.Add("Levels", filter.Levels.Select(level => (byte) level));
                filters.Add("pn.[LevelId] IN @Levels");
            }

            if (filter.StartTimeRange?.HasRange == true)
            {
                @params.Add("StartTimeMin", filter.StartTimeRange.Min);
                @params.Add("StartTimeMax", filter.StartTimeRange.Max);
                filters.Add("pn.[StartTime] BETWEEN @StartTimeMin AND @StartTimeMax");
            }

            if (filter.StopTimeRange?.HasRange == true)
            {
                @params.Add("StopTimeMin", filter.StopTimeRange.Min);
                @params.Add("StopTimeMax", filter.StopTimeRange.Max);
                filters.Add("pn.[StopTime] BETWEEN @StopTimeMin AND @StopTimeMax");
            }

            if (filter.IsActive != null)
            {
                @params.Add("Now", DateTime.Now.ToUniversalTime());
                filters.Add($"@Now {(filter.IsActive == false ? "NOT " : "")}BETWEEN pn.[StartTime] AND pn.[StopTime]");
            }

            return (@params, "", filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }
    }
}