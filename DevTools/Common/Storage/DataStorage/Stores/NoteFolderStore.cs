using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface INoteFolderStore
    {
        #region Folder

        Task<IEnumerable<FolderEntity>> GetFolders(IOperation operation);
        Task UpdateFolderPositions(IOperation operation, IEnumerable<FolderShortEntity> folders);
        Task<bool> IsFolderExist(IOperation operation, int folderId);
        Task<string> GetFolderName(IOperation operation, int folderId);
        Task<int> CreateFolder(IOperation operation, int folderId);
        Task UpdateFolder(IOperation operation, int folderId, string name);
        Task DeleteFolder(IOperation operation, int parentId, int folderId, IEnumerable<int> child);

        #endregion

        #region Notes

        Task<(long total, IEnumerable<NoteEntity> notes)> GetNotes(IOperation operation, NotePagingRequest request);
        Task<IEnumerable<NoteProjectEntity>> GetNotesProjects(IOperation operation, IEnumerable<int> noteIds);
        Task<bool> IsNoteExist(IOperation operation, int folderId, int noteId);
        Task<NoteExtendedEntity> GetNoteById(IOperation operation, int noteId);
        Task ChangeNoteFolder(IOperation operation, int noteId, int newFolderId);
        Task<int> CreateNote(IOperation operation, int folderId, int userId, NoteUpdateEntity entity);
        Task UpdateNote(IOperation operation, int noteId, NoteUpdateEntity entity);
        Task DeleteNoteById(IOperation operation, int noteId);
        Task<int> GetFolderNoteCount(IOperation operation, IEnumerable<int> folderIds);

        #endregion
    }

    public class NoteFolderStore : INoteFolderStore
    {
        #region Folders

        public async Task<IEnumerable<FolderEntity>> GetFolders(IOperation operation)
        {
            return await operation.Connection.QueryAsync<FolderEntity>(@"
                SELECT
                    [Id],
                    [Name],
                    [Nodes]
                FROM [simple-note-service].[Folder];
            ");
        }

        public async Task UpdateFolderPositions(IOperation operation, IEnumerable<FolderShortEntity> folders)
        {
            await operation.Connection.ExecuteAsync(new { Folders = folders.ToTable() }, @"
                UPDATE t
                SET t.[Nodes] = s.[Nodes]
                FROM [simple-note-service].[Folder] t
                INNER JOIN @Folders s ON s.[Id] = t.[Id];
            ");
        }

        public async Task<bool> IsFolderExist(IOperation operation, int folderId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = folderId }, @"
                SELECT TOP 1 1
                FROM [simple-note-service].[Folder]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<string> GetFolderName(IOperation operation, int folderId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<string>(new { Id = folderId }, @"
                SELECT [Name]
                FROM [simple-note-service].[Folder]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<int> CreateFolder(IOperation operation, int folderId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<int>(new { Id = folderId }, @"
                DECLARE @Ids TABLE ([Id] INT);

                INSERT INTO [simple-note-service].[Folder] ([Name])
                OUTPUT INSERTED.[Id] INTO @Ids
                VALUES ('New folder');

                DECLARE @NewId INT;
                SELECT @NewId = [Id] FROM @Ids;

                UPDATE [simple-note-service].[Folder]
                SET [Nodes] = IIF([Nodes] IS NULL, '[' + STR(@NewId) + ']', JSON_MODIFY([Nodes], 'append $', @NewId))
                WHERE [Id] = @Id;

                SELECT @NewId;
            ");
        }

        public async Task UpdateFolder(IOperation operation, int folderId, string name)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = folderId,
                Name = name
            }, @"
                UPDATE [simple-note-service].[Folder]
                SET [Name] = @Name
                WHERE [Id] = @Id;
            ");
        }

        public async Task DeleteFolder(IOperation operation, int parentId, int folderId, IEnumerable<int> child)
        {
            await operation.Connection.ExecuteAsync(new
            {
                ParentId = parentId,
                Id = folderId,
                Child = child.ToIntTable()
            }, @"
                UPDATE f
                SET [Nodes] = IIF(LEN(array.[Value]) > 0, '[' + array.[Value] + ']', NULL)
                FROM [simple-note-service].[Folder] f
                CROSS APPLY (
                    SELECT STRING_AGG(s.[Id], ',') WITHIN GROUP (ORDER BY s.[Id]) [Value] 
                    FROM OPENJSON(f.[Nodes], '$') WITH ([Id] INT '$') s
                    WHERE s.[Id] <> @Id
                ) array
                WHERE f.[Id] = @ParentId;

                DELETE [simple-note-service].[Folder]
                FROM [simple-note-service].[Folder] f
                INNER JOIN @Child c ON c.[Value] = f.[Id];
            ");
        }

        #endregion

        #region Notes

        public async Task<(long total, IEnumerable<NoteEntity> notes)> GetNotes(IOperation operation, NotePagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [simple-note-service].[Note] sns
                LEFT JOIN [authentication].[User] au ON au.[Id] = sns.[UserId]
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    sns.[Id],
                    sns.[Title],
                    IIF(LEN(sns.[Text]) > 50, LEFT(sns.[Text], 50) + '...', sns.[Text]) [Text],
                    sns.[UserId],
                    au.[Color],
                    au.[FirstName],
                    au.[LastName],
                    sns.[EditTime]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY sns.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var notes = await reader.ReadAsync<NoteEntity>();
            return (total, notes);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(NotePagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            @params.Add("FolderId", filter.FolderId);
            filters.Add("sns.[FolderId] = @FolderId");

            if (!filter.Ids.IsNullOrEmpty())
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("sns.[Id] IN @Ids");
            }

            if (!filter.Titles.IsNullOrEmpty())
            {
                @params.Add("Titles", filter.Titles.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Titles t ON sns.[Title] LIKE t.[Value]");
            }

            if (!filter.UserIds.IsNullOrEmpty())
            {
                @params.Add("UserIds", filter.UserIds);
                filters.Add("sns.[UserId] IN @UserIds");
            }

            if (!filter.ProjectIds.IsNullOrEmpty())
            {
                @params.Add("ProjectIds", filter.ProjectIds);
                joins.Add(@"
                    INNER JOIN (
                        SELECT DISTINCT np.[NoteId]
                        FROM [simple-note-service].[NoteProject] np
                        WHERE np.[ProjectId] IN @ProjectIds
                    ) pr ON sns.[Id] = pr.[NoteId]
                ");
            }

            if (filter.EditTimeRange?.HasRange == true)
            {
                @params.Add("EditTimeMin", filter.EditTimeRange.Min);
                @params.Add("EditTimeMax", filter.EditTimeRange.Max);
                filters.Add("sns.[EditTime] BETWEEN @EditTimeMin AND @EditTimeMax");
            }

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }

        public async Task<IEnumerable<NoteProjectEntity>> GetNotesProjects(IOperation operation, IEnumerable<int> noteIds)
        {
            return await operation.Connection.QueryAsync<NoteProjectEntity>(new { NoteIds = noteIds.ToIntTable() }, @"
                SELECT
                    np.[Id],
                    np.[NoteId],
                    np.[ProjectId],
                    p.[Name] [ProjectName],
                    p.[Alias] [ProjectAlias]
                FROM [simple-note-service].[NoteProject] np
                LEFT JOIN [portal].[Project] p ON np.[ProjectId] = p.[Id]
                INNER JOIN @NoteIds i ON np.[NoteId] = i.[Value];
            ");
        }

        public async Task<bool> IsNoteExist(IOperation operation, int folderId, int noteId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new
            {
                FolderId = folderId,
                Id = noteId
            }, @"
                SELECT TOP 1 1
                FROM [simple-note-service].[Note]
                WHERE [FolderId] = @FolderId AND [Id] = @Id;
            ");
        }

        public async Task<NoteExtendedEntity> GetNoteById(IOperation operation, int noteId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<NoteExtendedEntity>(new { Id = noteId }, @"
                SELECT
                    sns.[Id],
                    sns.[Title],
                    sns.[Text],
                    sns.[UserId],
                    au.[Color],
                    au.[FirstName],
                    au.[LastName],
                    sns.[EditTime],
                    sns.[FolderId],
                    f.[Name] [FolderName]
                FROM [simple-note-service].[Note] sns
                INNER JOIN [simple-note-service].[Folder] f ON f.[Id] = sns.[FolderId]
                LEFT JOIN [authentication].[User] au ON au.[Id] = sns.[UserId]
                WHERE sns.[Id] = @Id;
            ");
        }

        public async Task ChangeNoteFolder(IOperation operation, int noteId, int newFolderId)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = noteId,
                NewFolderId = newFolderId
            }, @"
                UPDATE [simple-note-service].[Note]
                SET [FolderId] = @NewFolderId
                WHERE [Id] = @Id;
            ");
        }

        public async Task<int> CreateNote(IOperation operation, int folderId, int userId, NoteUpdateEntity entity)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<int>(new
            {
                FolderId = folderId,
                UserId = userId,
                entity.Title,
                entity.Text,
                ProjectIds = entity.ProjectIds.ToIntTable()
            }, @"
                DECLARE @Ids TABLE ([Id] INT);

                INSERT INTO [simple-note-service].[Note] ([FolderId], [UserId], [Title], [Text])
                OUTPUT INSERTED.[Id] INTO @Ids
                VALUES (@FolderId, @UserId, @Title, @Text);

                DECLARE @Id INT;
                SELECT @Id = [Id] FROM @Ids;

                INSERT INTO [simple-note-service].[NoteProject] ([NoteId], [ProjectId])
                SELECT @Id, [Value]
                FROM @ProjectIds;

                SELECT @Id;
            ");
        }

        public async Task UpdateNote(IOperation operation, int noteId, NoteUpdateEntity entity)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = noteId,
                entity.Title,
                entity.Text,
                ProjectIds = entity.ProjectIds.ToIntTable()
            }, @"
                UPDATE [simple-note-service].[Note]
                SET
                    [Title] = @Title,
                    [Text] = @Text
                WHERE [Id] = @Id;

                MERGE [simple-note-service].[NoteProject] t
                USING @ProjectIds s
                ON t.[NoteId] = @Id AND t.[ProjectId] = s.[Value]
                WHEN NOT MATCHED THEN
                    INSERT ([NoteId], [ProjectId])
                    VALUES (@Id, s.[Value])
                WHEN NOT MATCHED BY SOURCE AND t.[NoteId] = @Id THEN
                    DELETE;
            ");
        }

        public async Task DeleteNoteById(IOperation operation, int noteId)
        {
            await operation.Connection.ExecuteAsync(new { Id = noteId }, @"
                DELETE FROM [simple-note-service].[Note]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<int> GetFolderNoteCount(IOperation operation, IEnumerable<int> folderIds)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<int>(new { FolderIds = folderIds.ToIntTable() }, @"
                SELECT COUNT(n.[Id])
                FROM [simple-note-service].[Note] n
                INNER JOIN @FolderIds i ON i.[Value] = n.[FolderId];
            ");
        }

        #endregion
    }
}