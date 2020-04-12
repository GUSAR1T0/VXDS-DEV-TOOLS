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
    }

    public class NoteStore : INoteStore
    {
        public async Task<(long total, IEnumerable<NoteEntity> notes)> Get(IOperation operation, NotePagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [module].[SimpleNoteService] sns
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

            if (filter.EditTimeRange?.HasRange == true)
            {
                @params.Add("EditTimeMin", filter.EditTimeRange.Min);
                @params.Add("EditTimeMax", filter.EditTimeRange.Max);
                filters.Add("sns.[EditTime] BETWEEN @EditTimeMin AND @EditTimeMax");
            }

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }
    }
}