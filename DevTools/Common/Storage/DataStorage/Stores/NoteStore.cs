using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Note;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface INoteStore
    {
        Task<(long total, IEnumerable<NoteEntity> notes)> Get(IOperation operation, NotePagingRequest request);
        Task<IEnumerable<NoteProjectEntity>> GetNotesProjects(IOperation operation, IEnumerable<int> noteIds);
        Task<bool> IsNoteExist(IOperation operation, int id);
        Task DeleteNoteById(IOperation operation, int id);
    }

    public class NoteStore : INoteStore
    {
        public async Task<(long total, IEnumerable<NoteEntity> notes)> Get(IOperation operation, NotePagingRequest request)
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
            var operations = await reader.ReadAsync<NoteEntity>();
            return (total, operations);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(NotePagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            if (filter.Ids?.Any() == true)
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("sns.[Id] IN @Ids");
            }

            if (filter.Titles?.Any() == true)
            {
                @params.Add("Titles", filter.Titles.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Titles t ON sns.[Title] LIKE t.[Value]");
            }

            if (filter.UserIds?.Any() == true)
            {
                @params.Add("UserIds", filter.UserIds);
                filters.Add("sns.[UserId] IN @UserIds");
            }

            if (filter.ProjectIds?.Any() == true)
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

        public async Task<bool> IsNoteExist(IOperation operation, int id)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = id }, @"
                SELECT TOP 1 1
                FROM [simple-note-service].[Note]
                WHERE [Id] = @Id;
            ");
        }

        public async Task DeleteNoteById(IOperation operation, int id)
        {
            await operation.Connection.ExecuteAsync(new { Id = id }, @"
                DELETE FROM [simple-note-service].[Note]
                WHERE [Id] = @Id;
            ");
        }
    }
}