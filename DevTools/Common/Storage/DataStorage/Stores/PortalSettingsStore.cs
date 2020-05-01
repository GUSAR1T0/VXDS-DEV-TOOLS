using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IPortalSettingsStore
    {
        #region Hosts

        Task<(long total, IEnumerable<HostSettingsItemEntity> hosts)> GetHosts(IOperation operation, HostPagingRequest request);
        Task<bool> IsHostExist(IOperation operation, int hostId);
        Task AddHost(IOperation operation, HostSettingsItemEntity host);
        Task UpdateHost(IOperation operation, HostSettingsItemEntity host);
        Task DeleteHost(IOperation operation, int hostId);

        #endregion

        #region General Oriented Settings

        Task<IEnumerable<SettingsParametersItemEntity>> GetSettingsParameters(IOperation operation, params string[] keys);
        Task<string> GetSettingsParameter(IOperation operation, string name);
        Task ModifySettings(IOperation operation, string name, string value);

        #endregion
    }

    public class PortalSettingsStore : BaseDataStore, IPortalSettingsStore
    {
        #region Hosts

        public async Task<(long total, IEnumerable<HostSettingsItemEntity> hosts)> GetHosts(IOperation operation, HostPagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [portal].[Host] ph
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    ph.[Id],
                    ph.[Name],
                    ph.[Domain],
                    ph.[OperationSystemId] [OperationSystem],
                    ph.[Credentials]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY ph.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var hosts = await reader.ReadAsync<HostSettingsItemEntity>();
            return (total, hosts);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(HostPagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            if (!filter.Ids.IsNullOrEmpty())
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("ph.[Id] IN @Ids");
            }

            if (!filter.Names.IsNullOrEmpty())
            {
                @params.Add("Names", filter.Names.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Names name ON ph.[Name] LIKE name.[Value]");
            }

            if (!filter.Domains.IsNullOrEmpty())
            {
                @params.Add("Domains", filter.Domains.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Domains domain ON ph.[Domain] LIKE domain.[Value]");
            }

            if (!filter.OperationSystems.IsNullOrEmpty())
            {
                @params.Add("OperationSystemIds", filter.OperationSystems);
                filters.Add("ph.[OperationSystemId] IN @OperationSystemIds");
            }

            filters.Add("ph.[IsActive] = 1");

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }

        public async Task<bool> IsHostExist(IOperation operation, int hostId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = hostId }, @"
                SELECT TOP 1 1
                FROM [portal].[Host]
                WHERE [Id] = @Id;
            ");
        }

        public async Task AddHost(IOperation operation, HostSettingsItemEntity host)
        {
            await operation.Connection.ExecuteAsync(new
            {
                host.Name,
                host.Domain,
                host.OperationSystem,
                host.Credentials
            }, @"
                INSERT INTO [portal].[Host] ([Name], [Domain], [OperationSystemId], [Credentials])
                VALUES (@Name, @Domain, @OperationSystem, @Credentials);
            ");
        }

        public async Task UpdateHost(IOperation operation, HostSettingsItemEntity host)
        {
            await operation.Connection.ExecuteAsync(new
            {
                host.Id,
                host.Name,
                host.Domain,
                host.OperationSystem,
                host.Credentials
            }, @"
                UPDATE [portal].[Host]
                SET
                    [Name] = @Name,
                    [Domain] = @Domain,
                    [OperationSystemId] = @OperationSystem,
                    [Credentials] = @Credentials
                WHERE [Id] = @Id;
            ");
        }

        public async Task DeleteHost(IOperation operation, int hostId)
        {
            await operation.Connection.ExecuteAsync(new { Id = hostId }, @"
                DELETE FROM [portal].[Host]
                WHERE [Id] = @Id;
            ");
        }

        #endregion

        #region General Oriented Settings

        public async Task<IEnumerable<SettingsParametersItemEntity>> GetSettingsParameters(IOperation operation, params string[] keys)
        {
            return await operation.Connection.QueryAsync<SettingsParametersItemEntity>(new { Keys = keys }, @"
                SELECT
                    [Name],
                    [Value]
                FROM [portal].[Settings]
                WHERE [Name] IN @Keys;
            ");
        }

        public async Task<string> GetSettingsParameter(IOperation operation, string name)
        {
            return await operation.Connection.QueryFirstOrDefaultAsync<string>(new { Name = name }, @"
                SELECT [Value]
                FROM [portal].[Settings]
                WHERE @Name = [Name];
            ");
        }

        public async Task ModifySettings(IOperation operation, string name, string value)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Name = name,
                Value = value
            }, @"
                MERGE [portal].[Settings] AS target
                USING (
                    SELECT
                        @Name  [Name],
                        @Value [Value]
                ) AS source
                ON target.[Name] = source.[Name]
                WHEN MATCHED THEN UPDATE SET
                    target.[Value] = source.[Value]
                WHEN NOT MATCHED THEN INSERT ([Name], [Value])
                    VALUES (source.[Name], source.[Value]);
            ");
        }

        #endregion
    }
}