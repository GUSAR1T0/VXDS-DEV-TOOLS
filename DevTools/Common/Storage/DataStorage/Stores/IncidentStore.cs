using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IIncidentStore
    {
        Task<IncidentBaseEntity> GetIncident(IOperation operation, long operationId);
        Task<IncidentFullEntity> GetIncidentWithHistory(IOperation operation, long operationId);
        Task<(long total, IEnumerable<IncidentListItem> incidents)> GetIncidents(IOperation operation, IncidentPagingRequest request);
        Task<bool> IsIncidentExist(IOperation operation, long operationId);
        Task InitializeIncident(IOperation operation, IncidentUpdateEntity entity);
        Task UpdateIncident(IOperation operation, IncidentUpdateEntity entity, IncidentHistoryRecordEntity history);
    }

    public class IncidentStore : BaseDataStore, IIncidentStore
    {
        public async Task<IncidentBaseEntity> GetIncident(IOperation operation, long operationId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<IncidentBaseEntity>(new { OperationId = operationId }, @"
                SELECT
                    [OperationId],
                    [AuthorId],
                    [AssigneeId],
                    [StatusId] AS [Status]
                FROM [portal].[Incident]
                WHERE [OperationId] = @OperationId;
            ");
        }

        public async Task<IncidentFullEntity> GetIncidentWithHistory(IOperation operation, long operationId)
        {
            var reader = await operation.Connection.QueryMultipleAsync(new { OperationId = operationId }, $@"
                SELECT
                    pin.[OperationId],
                    bo.[Scope],
                    bo.[ContextName],
                    pin.[AuthorId],
                    author.[Color] AS [AuthorColor],
                    author.[FirstName] AS [AuthorFirstName],
                    author.[LastName] AS [AuthorLastName],
                    pin.[AssigneeId],
                    assignee.[Color] AS [AssigneeColor],
                    assignee.[FirstName] AS [AssigneeFirstName],
                    assignee.[LastName] AS [AssigneeLastName],
                    pin.[InitialTime],
                    pin.[StatusId] AS [Status]
                FROM [portal].[Incident] pin
                INNER JOIN [base].[Operation] bo ON bo.[Id] = pin.[OperationId]
                INNER JOIN [authentication].[User] author ON author.[Id] = pin.[AuthorId]
                LEFT JOIN [authentication].[User] assignee ON assignee.[Id] = pin.[AssigneeId]
                WHERE pin.[OperationId] = @OperationId;

                SELECT
                    pinh.[OperationId],
                    pinh.[AuthorId],
                    author.[Color] AS [AuthorColor],
                    author.[FirstName] AS [AuthorFirstName],
                    author.[LastName] AS [AuthorLastName],
                    pinh.[AssigneeId],
                    pinh.[IsUnassigned],
                    assignee.[Color] AS [AssigneeColor],
                    assignee.[FirstName] AS [AssigneeFirstName],
                    assignee.[LastName] AS [AssigneeLastName],
                    pinh.[ChangeTime],
                    pinh.[StatusId] AS [Status],
                    pinh.[Comment]
                FROM [portal].[IncidentHistory] pinh
                LEFT JOIN [authentication].[User] author ON author.[Id] = pinh.[AuthorId]
                LEFT JOIN [authentication].[User] assignee ON assignee.[Id] = pinh.[AssigneeId]
                WHERE pinh.[OperationId] = @OperationId;
            ");
            var incident = await reader.ReadSingleOrDefaultAsync<IncidentFullEntity>();
            incident.History = await reader.ReadAsync<IncidentHistoryEntity>();
            return incident;
        }

        public async Task<(long total, IEnumerable<IncidentListItem> incidents)> GetIncidents(IOperation operation, IncidentPagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [portal].[Incident] pin
                INNER JOIN [base].[Operation] bo ON bo.[Id] = pin.[OperationId]
                INNER JOIN [authentication].[User] author ON author.[Id] = pin.[AuthorId]
                LEFT JOIN [authentication].[User] assignee ON assignee.[Id] = pin.[AssigneeId]
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    pin.[OperationId],
                    bo.[Scope],
                    bo.[ContextName],
                    pin.[AuthorId],
                    author.[Color] AS [AuthorColor],
                    author.[FirstName] AS [AuthorFirstName],
                    author.[LastName] AS [AuthorLastName],
                    pin.[AssigneeId],
                    assignee.[Color] AS [AssigneeColor],
                    assignee.[FirstName] AS [AssigneeFirstName],
                    assignee.[LastName] AS [AssigneeLastName],
                    pin.[InitialTime],
                    pin.[StatusId] AS [Status]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY pin.[OperationId]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var users = await reader.ReadAsync<IncidentListItem>();
            return (total, users);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(IncidentPagingFilter filter)
        {
            var @params = new DynamicParameters();
            var filters = new List<string>();

            if (filter.OperationIds?.Any() == true)
            {
                @params.Add("OperationIds", filter.OperationIds);
                filters.Add("pin.[OperationId] IN @OperationIds");
            }

            if (filter.AuthorIds?.Any() == true)
            {
                @params.Add("AuthorIds", filter.AuthorIds);
                filters.Add("pin.[AuthorId] IN @AuthorIds");
            }

            if (filter.AssigneeIds?.Any() == true)
            {
                var assigneeIds = new List<int>(filter.AssigneeIds);
                var assigneeIdsFilter = "pin.[AssigneeId] IN @AssigneeIds";

                if (filter.AssigneeIds.Contains(0))
                {
                    assigneeIds.Remove(0);
                    assigneeIdsFilter = $"(pin.[AssigneeId] IS NULL OR {assigneeIdsFilter})";
                }

                @params.Add("AssigneeIds", assigneeIds);
                filters.Add(assigneeIdsFilter);
            }

            if (filter.InitialTimeRange?.HasRange == true)
            {
                @params.Add("InitialTimeMin", filter.InitialTimeRange.Min);
                @params.Add("InitialTimeMax", filter.InitialTimeRange.Max);
                filters.Add("pin.[InitialTime] BETWEEN @InitialTimeMin AND @InitialTimeMax");
            }

            if (filter.Statuses?.Any() == true)
            {
                @params.Add("StatusIds", filter.Statuses.Select(status => (byte) status));
                filters.Add("pin.[StatusId] IN @StatusIds");
            }

            return (@params, "", filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }

        public async Task<bool> IsIncidentExist(IOperation operation, long operationId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { OperationId = operation }, @"
                SELECT TOP 1 1
                FROM [portal].[Incident]
                WHERE [OperationId] = @OperationId;
            ");
        }

        public async Task InitializeIncident(IOperation operation, IncidentUpdateEntity entity)
        {
            await operation.Connection.ExecuteAsync(entity, @"
                INSERT INTO [portal].[Incident] (
                    [OperationId],
                    [AuthorId],
                    [AssigneeId],
                    [StatusId]
                )
                VALUES (
                    @OperationId,
                    @AuthorId,
                    @AssigneeId,
                    @Status
                );

                INSERT INTO [portal].[IncidentHistory] (
                    [OperationId],
                    [AuthorId],
                    [AssigneeId],
                    [IsUnassigned],
                    [StatusId],
                    [Comment]
                )
                VALUES (
                    @OperationId,
                    @AuthorId,
                    @AssigneeId,
                    IIF(@AssigneeId IS NULL, 0, 1),
                    @Status,
                    @Comment
                );
            ");
        }

        public async Task UpdateIncident(IOperation operation, IncidentUpdateEntity entity, IncidentHistoryRecordEntity history)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.OperationId,
                OriginalAuthorId = entity.AuthorId,
                OriginalAssigneeId = entity.AssigneeId,
                OriginalStatus = entity.Status,
                entity.Comment,
                HistoricalAuthorId = history.AuthorId,
                HistoricalAssigneeId = history.AssigneeId,
                history.IsUnassigned,
                HistoricalStatus = history.Status
            }, @"
                UPDATE
                SET
                    [AuthorId] = @OriginalAuthorId,
                    [AssigneeId] = @OriginalAssigneeId,
                    [Status] = @OriginalStatus
                FROM [portal].[Incident]
                WHERE [OperationId] = @OperationId;

                INSERT INTO [portal].[IncidentHistory] (
                    [OperationId],
                    [AuthorId],
                    [AssigneeId],
                    [IsUnassigned],
                    [StatusId],
                    [Comment]
                )
                VALUES (
                    @OperationId,
                    @HistoricalAuthorId,
                    @HistoricalAssigneeId,
                    @IsUnassigned,
                    @HistoricalStatus,
                    @Comment
                );
            ");
        }
    }
}