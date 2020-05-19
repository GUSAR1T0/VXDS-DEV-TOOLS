using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Notification;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface INotificationStore
    {
        Task<(long total, IEnumerable<NotificationEntity> notifications)> Get(IOperation operation, NotificationPagingRequest request);
        Task ModifyNotification(IOperation operation, NotificationUpdateEntity entity);
        Task<bool> IsNotificationExist(IOperation operation, int id);
        Task DeleteNotificationById(IOperation operation, int id);
    }

    public class NotificationStore : BaseDataStore, INotificationStore
    {
        public async Task<(long total, IEnumerable<NotificationEntity> notifications)> Get(IOperation operation, NotificationPagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [portal].[Notification] pn
                LEFT JOIN [authentication].[User] au ON au.[Id] = pn.[UserId]
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    pn.[Id],
                    pn.[Message],
                    pn.[LevelId] AS [Level],
                    pn.[StartTime],
                    pn.[StopTime],
                    pn.[UserId],
                    au.[Color],
                    au.[FirstName],
                    au.[LastName]
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

            if (!filter.Ids.IsNullOrEmpty())
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("pn.[Id] IN @Ids");
            }

            if (!filter.Levels.IsNullOrEmpty())
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

            if (!filter.UserIds.IsNullOrEmpty())
            {
                @params.Add("UserIds", filter.UserIds);
                filters.Add("pn.[UserId] IN @UserIds");
            }

            return (@params, "", filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }

        public async Task ModifyNotification(IOperation operation, NotificationUpdateEntity entity)
        {
            await operation.Connection.ExecuteAsync(entity, @"
                MERGE [portal].[Notification] AS target
                USING (
                    SELECT
                        @Id        [Id],
                        @Message   [Message],
                        @Level     [LevelId],
                        @StartTime [StartTime],
                        @StopTime  [StopTime],
                        @UserId    [UserId]
                ) AS source
                ON target.[Id] = source.[Id]
                WHEN MATCHED THEN UPDATE SET
                    target.[Message] = source.[Message],
                    target.[LevelId] = source.[LevelId],
                    target.[StartTime] = source.[StartTime],
                    target.[StopTime] = source.[StopTime]
                WHEN NOT MATCHED THEN INSERT ([Message], [LevelId], [StartTime], [StopTime], [UserId])
                    VALUES (source.[Message], source.[LevelId], source.[StartTime], source.[StopTime], source.[UserId]);
            ");
        }

        public async Task<bool> IsNotificationExist(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = id }, @"
                SELECT TOP 1 1
                FROM [portal].[Notification]
                WHERE [Id] = @Id;
            ");
        }

        public async Task DeleteNotificationById(IOperation operation, int id)
        {
            await operation.Connection.ExecuteAsync(new { Id = id }, @"
                DELETE FROM [portal].[Notification]
                WHERE [Id] = @Id;
            ");
        }
    }
}