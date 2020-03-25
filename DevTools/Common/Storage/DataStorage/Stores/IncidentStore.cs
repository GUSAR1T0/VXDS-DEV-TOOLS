using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Core.Entities.Incident;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IIncidentStore
    {
        Task<IncidentBaseEntity> GetIncident(IOperation operation, long operationId);
        Task<IncidentFullEntity> GetIncidentWithHistory(IOperation operation, long operationId);
        Task<bool> IsIncidentExist(IOperation operation, long operationId);
        Task InitializeIncident(IOperation operation, IncidentUpdateEntity entity);
        Task SaveComment(IOperation operation, long operationId, int? userId, int? historyId, string comment);
        Task DeleteComment(IOperation operation, long historyId);
        Task UpdateIncident(IOperation operation, IncidentUpdateEntity entity, IncidentHistoryRecordEntity history);
    }

    public class IncidentStore : BaseDataStore, IIncidentStore
    {
        public async Task<IncidentBaseEntity> GetIncident(IOperation operation, long operationId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<IncidentBaseEntity>(new { IncidentOperationId = operationId }, @"
                SELECT
                    [OperationId],
                    [AuthorId],
                    [AssigneeId],
                    [StatusId] AS [Status]
                FROM [portal].[Incident]
                WHERE [OperationId] = @IncidentOperationId;
            ");
        }

        public async Task<IncidentFullEntity> GetIncidentWithHistory(IOperation operation, long operationId)
        {
            var reader = await operation.Connection.QueryMultipleAsync(new { IncidentOperationId = operationId }, $@"
                SELECT
                    pin.[OperationId] AS [IncidentOperationId],
                    bo.[Scope],
                    bo.[ContextName],
                    bo.[IsSuccessful],
                    bo.[IsSystemAction],
                    bo.[UserId],
                    operationUser.[Color],
                    operationUser.[FirstName],
                    operationUser.[LastName],
                    bo.[StartTime],
                    bo.[StopTime],
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
                LEFT JOIN [authentication].[User] operationUser ON operationUser.[Id] = bo.[UserId]
                INNER JOIN [authentication].[User] author ON author.[Id] = pin.[AuthorId]
                LEFT JOIN [authentication].[User] assignee ON assignee.[Id] = pin.[AssigneeId]
                WHERE pin.[OperationId] = @IncidentOperationId;

                SELECT
                    pinh.[Id],
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
                    pinh.[ChangedBy],
                    changedBy.[Color] AS [ChangedByColor],
                    changedBy.[FirstName] AS [ChangedByFirstName],
                    changedBy.[LastName] AS [ChangedByLastName],
                    pinh.[ChangeTime],
                    pinh.[StatusId] AS [Status],
                    pinh.[Comment]
                FROM [portal].[IncidentHistory] pinh
                LEFT JOIN [authentication].[User] author ON author.[Id] = pinh.[AuthorId]
                LEFT JOIN [authentication].[User] assignee ON assignee.[Id] = pinh.[AssigneeId]
                LEFT JOIN [authentication].[User] changedBy ON changedBy.[Id] = pinh.[ChangedBy]
                WHERE pinh.[OperationId] = @IncidentOperationId
                ORDER BY pinh.[ChangeTime] DESC;
            ");
            var incident = await reader.ReadSingleOrDefaultAsync<IncidentFullEntity>();
            incident.History = await reader.ReadAsync<IncidentHistoryEntity>();
            return incident;
        }

        public async Task<bool> IsIncidentExist(IOperation operation, long operationId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { IncidentOperationId = operationId }, @"
                SELECT TOP 1 1
                FROM [portal].[Incident]
                WHERE [OperationId] = @IncidentOperationId;
            ");
        }

        public async Task InitializeIncident(IOperation operation, IncidentUpdateEntity entity)
        {
            await operation.Connection.ExecuteAsync(entity, @"
                INSERT INTO [portal].[Incident] (
                    [OperationId],
                    [AuthorId],
                    [AssigneeId]
                )
                VALUES (
                    @IncidentOperationId,
                    @AuthorId,
                    @AssigneeId
                );

                INSERT INTO [portal].[IncidentHistory] (
                    [OperationId],
                    [AuthorId],
                    [AssigneeId],
                    [IsUnassigned],
                    [StatusId],
                    [ChangedBy],
                    [Comment]
                )
                VALUES (
                    @IncidentOperationId,
                    @AuthorId,
                    @AssigneeId,
                    IIF(@AssigneeId IS NULL, 0, 1),
                    1,
                    @ChangedBy,
                    @Comment
                );
            ");
        }

        public async Task SaveComment(IOperation operation, long operationId, int? userId, int? historyId, string comment)
        {
            await operation.Connection.ExecuteAsync(new
            {
                IncidentOperationId = operationId,
                ChangedBy = userId,
                Id = historyId,
                Comment = comment
            }, @"
                MERGE [portal].[IncidentHistory] t
                USING (SELECT @Id [Id]) s
                ON t.[Id] = s.[Id]
                WHEN MATCHED AND t.[ChangedBy] = @ChangedBy THEN
                    UPDATE SET
                        t.[Comment] = @Comment
                WHEN NOT MATCHED THEN
                    INSERT ([OperationId], [ChangedBy], [Comment])
                    VALUES (@IncidentOperationId, @ChangedBy, @Comment);
            ");
        }

        public async Task DeleteComment(IOperation operation, long historyId)
        {
            await operation.Connection.ExecuteAsync(new { Id = historyId }, @"
                IF EXISTS (
                    SELECT 1
                    FROM [portal].[IncidentHistory]
                    WHERE
                        [Id] = @Id AND
                        [AuthorId] IS NULL AND
                        [AssigneeId] IS NULL AND
                        [IsUnassigned] IS NULL AND
                        [StatusId] IS NULL
                )
                BEGIN

                    DELETE FROM [portal].[IncidentHistory] WHERE [Id] = @Id;

                END
                ELSE
                BEGIN

                    UPDATE pinh
                    SET pinh.[Comment] = NULL
                    FROM [portal].[IncidentHistory] pinh
                    WHERE pinh.[Id] = @Id;

                END
            ");
        }

        public async Task UpdateIncident(IOperation operation, IncidentUpdateEntity entity, IncidentHistoryRecordEntity history)
        {
            await operation.Connection.ExecuteAsync(new
            {
                entity.IncidentOperationId,
                OriginalAuthorId = entity.AuthorId,
                OriginalAssigneeId = entity.AssigneeId,
                OriginalStatus = entity.Status,
                HistoricalAuthorId = history.AuthorId,
                HistoricalAssigneeId = history.AssigneeId,
                history.IsUnassigned,
                HistoricalStatus = history.Status,
                entity.ChangedBy,
                entity.Comment
            }, @"
                UPDATE pin
                SET
                    pin.[AuthorId] = @OriginalAuthorId,
                    pin.[AssigneeId] = @OriginalAssigneeId,
                    pin.[StatusId] = @OriginalStatus
                FROM [portal].[Incident] pin
                WHERE pin.[OperationId] = @IncidentOperationId;

                INSERT INTO [portal].[IncidentHistory] (
                    [OperationId],
                    [AuthorId],
                    [AssigneeId],
                    [IsUnassigned],
                    [StatusId],
                    [ChangedBy],
                    [Comment]
                )
                VALUES (
                    @IncidentOperationId,
                    @HistoricalAuthorId,
                    @HistoricalAssigneeId,
                    @IsUnassigned,
                    @HistoricalStatus,
                    @ChangedBy,
                    @Comment
                );
            ");
        }
    }
}