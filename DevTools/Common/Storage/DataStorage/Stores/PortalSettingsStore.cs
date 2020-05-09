using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Constants;
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
        Task<IEnumerable<HostSettingsEntity>> SearchHostsByPattern(IOperation operation, string pattern, IEnumerable<HostOperatingSystem> operatingSystems);
        Task<bool> IsHostExist(IOperation operation, int hostId);
        Task<bool> IsHostNameUnique(IOperation operation, int hostId, string hostName);
        Task<HostSettingsItemEntity> GetHost(IOperation operation, int hostId);
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
                    ph.[OperatingSystemId] [OperatingSystem],
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

            if (!filter.OperatingSystems.IsNullOrEmpty())
            {
                @params.Add("OperatingSystemIds", filter.OperatingSystems);
                filters.Add("ph.[OperatingSystemId] IN @OperatingSystemIds");
            }

            filters.Add("ph.[IsActive] = 1");

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }

        public async Task<IEnumerable<HostSettingsEntity>> SearchHostsByPattern(IOperation operation, string pattern, IEnumerable<HostOperatingSystem> operatingSystems)
        {
            var operatingSystemList = operatingSystems.ToList();
            return await operation.Connection.QueryAsync<HostSettingsEntity>(new
            {
                Pattern = $"%{pattern}%",
                OperatingSystems = operatingSystemList.Select(item => (int) item).ToIntTable()
            }, $@"
                SELECT TOP {FormatPattern.SearchMaxCount}
                    ph.[Id],
                    ph.[Name],
                    ph.[Domain],
                    ph.[OperatingSystemId] AS [OperatingSystem]
                FROM [portal].[Host] ph
                {(operatingSystemList.Any() ? "INNER JOIN @OperatingSystems os ON os.[Value] = ph.[OperatingSystemId]" : "")} 
                WHERE ph.[Name] LIKE @Pattern OR ph.[Domain] LIKE @Pattern;
            ");
        }

        public async Task<bool> IsHostExist(IOperation operation, int hostId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = hostId }, @"
                SELECT TOP 1 1
                FROM [portal].[Host]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<bool> IsHostNameUnique(IOperation operation, int hostId, string hostName)
        {
            return !await operation.Connection.QuerySingleOrDefaultAsync<bool>(new
            {
                Id = hostId,
                Name = hostName
            }, @"
                SELECT TOP 1 1
                FROM [portal].[Host]
                WHERE (@Id = 0 OR [Id] <> @Id) AND [Name] = @Name;
            ");
        }

        public async Task<HostSettingsItemEntity> GetHost(IOperation operation, int hostId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<HostSettingsItemEntity>(new { Id = hostId }, @"
                SELECT
                    [Id],
                    [Name],
                    [Domain],
                    [OperatingSystemId] [OperatingSystem],
                    [Credentials]
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
                host.OperatingSystem,
                host.Credentials
            }, @"
                INSERT INTO [portal].[Host] ([Name], [Domain], [OperatingSystemId], [Credentials])
                VALUES (@Name, @Domain, @OperatingSystem, @Credentials);
            ");
        }

        public async Task UpdateHost(IOperation operation, HostSettingsItemEntity host)
        {
            await operation.Connection.ExecuteAsync(new
            {
                host.Id,
                host.Name,
                host.Domain,
                host.OperatingSystem,
                host.Credentials
            }, @"
                UPDATE [portal].[Host]
                SET
                    [Name] = @Name,
                    [Domain] = @Domain,
                    [OperatingSystemId] = @OperatingSystem,
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