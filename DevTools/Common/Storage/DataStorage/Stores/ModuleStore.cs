using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Extensions;

namespace VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores
{
    public interface IModuleStore
    {
        Task<(long total, IEnumerable<ModuleListItemEntity> modules)> GetModules(IOperation operation, ModulePagingRequest request);
        Task<ModuleEntity> GetModule(IOperation operation, int moduleId);
        Task<IEnumerable<ModuleShortEntity>> GetModules(IOperation operation);
        Task<Dictionary<int, ModuleStatus>> GetHostModuleIds(IOperation operation, int hostId);
        Task<int> GetHostModuleCount(IOperation operation, int hostId);
        Task<bool> IsModuleExists(IOperation operation, int moduleId);
        Task<IEnumerable<ModuleStatus>> HasStatuses(IOperation operation, int moduleId, params ModuleStatus[] statuses);
        Task ChangeStatus(IOperation operation, int moduleId, ModuleStatus status);
        Task<ModuleInfoEntity> GetModuleByAlias(IOperation operation, string alias);
        Task<int> CreateModule(IOperation operation, int userId, int hostId, int fileId, ModuleConfigurationFile configuration);
        Task UpdateModule(IOperation operation, int moduleId, int userId);
        Task UpgradeModule(IOperation operation, int moduleId, int userId, int fileId, ModuleConfigurationFile configuration);
        Task UpgradeModule(IOperation operation, int moduleId, int userId, int configurationId);
        Task DowngradeModule(IOperation operation, int moduleId, int userId, int configurationId);
        Task DeleteModule(IOperation operation, int moduleId);
        Task<IEnumerable<ModuleHistoryEntity>> GetModuleHistory(IOperation operation, int moduleId);
    }

    public class ModuleStore : IModuleStore
    {
        public async Task<(long total, IEnumerable<ModuleListItemEntity> modules)> GetModules(IOperation operation, ModulePagingRequest request)
        {
            var selectBase = $@"
                SELECT {{0}} FROM [portal].[Module] pm
                INNER JOIN [portal].[ActiveModuleConfiguration] amc ON amc.[ModuleId] = pm.[Id]
                INNER JOIN [portal].[ModuleConfiguration] mc ON mc.[Id] = amc.[ModuleConfigurationId]
                INNER JOIN [authentication].[User] au ON au.[Id] = pm.[UserId]
                INNER JOIN [portal].[Host] ph ON ph.[Id] = pm.[HostId]
                {{1}}
                {{2}}
            ";
            const string selectEntity = @"
                    pm.[Id],
                    pm.[Alias],
                    mc.[Name],
                    mc.[Version],
                    pm.[UserId],
                    au.[Color],
                    au.[FirstName],
                    au.[LastName],
                    pm.[HostId],
                    ph.[Name] AS [HostName],
                    ph.[Domain] AS [HostDomain],
                    ph.[OperatingSystemId] AS [HostOperatingSystem],
                    pm.[StatusId] AS [Status]
            ";
            const string selectTotal = "COUNT_BIG(1)";
            var (@params, joins, filters) = HandleGetRequest(request.Filter);
            var query = $@"
                {string.Format(selectBase, selectTotal, joins, filters)};
                {string.Format(selectBase, selectEntity, joins, filters)}
                ORDER BY pm.[Id]
                OFFSET {request.PageNo * request.PageSize} ROWS FETCH NEXT {request.PageSize} ROWS ONLY;
            ";
            using var reader = await operation.Connection.QueryMultipleAsync(@params, query);
            var total = await reader.ReadSingleAsync<long>();
            var modules = await reader.ReadAsync<ModuleListItemEntity>();
            return (total, modules);
        }

        private static (DynamicParameters, string, string) HandleGetRequest(ModulePagingFilter filter)
        {
            var @params = new DynamicParameters();
            var joins = new List<string>();
            var filters = new List<string>();

            if (!filter.Ids.IsNullOrEmpty())
            {
                @params.Add("Ids", filter.Ids);
                filters.Add("pm.[Id] IN @Ids");
            }

            if (!filter.Names.IsNullOrEmpty())
            {
                @params.Add("Names", filter.Names.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Names name ON mc.[Name] LIKE name.[Value]");
            }

            if (!filter.Aliases.IsNullOrEmpty())
            {
                @params.Add("Aliases", filter.Aliases.Select(item => $"%{item}%").ToStringTable());
                joins.Add("INNER JOIN @Aliases alias ON pm.[Alias] LIKE alias.[Value]");
            }

            if (!filter.HostIds.IsNullOrEmpty())
            {
                @params.Add("HostIds", filter.HostIds);
                filters.Add("pm.[HostId] IN @HostIds");
            }

            if (!filter.UserIds.IsNullOrEmpty())
            {
                @params.Add("UserIds", filter.UserIds);
                filters.Add("pm.[UserId] IN @UserIds");
            }

            if (!filter.Statuses.IsNullOrEmpty())
            {
                @params.Add("StatusIds", filter.Statuses);
                filters.Add("pm.[StatusId] IN @StatusIds");
            }

            return (@params, string.Join(" ", joins), filters.Any() ? $"WHERE {string.Join(" AND ", filters)}" : "");
        }

        public async Task<ModuleEntity> GetModule(IOperation operation, int moduleId)
        {
            var reader = await operation.Connection.QueryMultipleAsync(new { Id = moduleId }, $@"
                SELECT
                    pm.[Id],
                    pm.[Alias],
                    mc.[Name],
                    mc.[Version],
                    pm.[UserId],
                    au.[Color],
                    au.[FirstName],
                    au.[LastName],
                    pm.[HostId],
                    ph.[Name] AS [HostName],
                    ph.[Domain] AS [HostDomain],
                    ph.[OperatingSystemId] AS [HostOperatingSystem],
                    pm.[StatusId] AS [Status]
                FROM [portal].[Module] pm
                INNER JOIN [portal].[ActiveModuleConfiguration] amc ON amc.[ModuleId] = pm.[Id]
                INNER JOIN [portal].[ModuleConfiguration] mc ON mc.[Id] = amc.[ModuleConfigurationId]
                INNER JOIN [authentication].[User] au ON au.[Id] = pm.[UserId]
                INNER JOIN [portal].[Host] ph ON ph.[Id] = pm.[HostId]
                WHERE pm.[Id] = @Id;

                SELECT
                    [Id],
                    [ModuleId],
                    [Name],
                    [Version],
                    [Author],
                    [Email],
                    [FileId]
                FROM [portal].[ModuleConfiguration]
                WHERE [ModuleId] = @Id
                ORDER BY [Version];
            ");
            var entity = await reader.ReadFirstOrDefaultAsync<ModuleEntity>();
            entity.Configurations = (await reader.ReadAsync<ModuleConfigurationEntity>()).ToList();
            return entity;
        }

        public async Task<IEnumerable<ModuleShortEntity>> GetModules(IOperation operation)
        {
            return await operation.Connection.QueryAsync<ModuleShortEntity>($@"
                SELECT
                    pm.[Id],
                    pm.[Alias],
                    mc.[Name],
                    mc.[Version],
                    pm.[StatusId] AS [Status]
                FROM [portal].[Module] pm
                INNER JOIN [portal].[ActiveModuleConfiguration] amc ON amc.[ModuleId] = pm.[Id]
                INNER JOIN [portal].[ModuleConfiguration] mc ON mc.[Id] = amc.[ModuleConfigurationId];
            ");
        }

        public async Task<Dictionary<int, ModuleStatus>> GetHostModuleIds(IOperation operation, int hostId)
        {
            var results = await operation.Connection.QueryAsync<(int, ModuleStatus)>(new { HostId = hostId }, @"
                SELECT [Id], [StatusId]
                FROM [portal].[Module]
                WHERE [HostId] = @HostId AND [StatusId] NOT IN (4, 8, 12, 13, 14, 15, 16);
            ");
            return results.ToDictionary(item => item.Item1, item => item.Item2);
        }

        public async Task<int> GetHostModuleCount(IOperation operation, int hostId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<int>(new { HostId = hostId }, @"
                SELECT COUNT(*)
                FROM [portal].[Module]
                WHERE [HostId] = @HostId AND [StatusId] NOT IN (4, 8, 12, 13, 14, 15, 16);
            ");
        }

        public async Task<bool> IsModuleExists(IOperation operation, int moduleId)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<bool>(new { Id = moduleId }, @"
                SELECT TOP 1 1
                FROM [portal].[Module]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<IEnumerable<ModuleStatus>> HasStatuses(IOperation operation, int moduleId, params ModuleStatus[] statuses)
        {
            return await operation.Connection.QueryAsync<ModuleStatus>(new
            {
                Id = moduleId,
                StatusIds = statuses
            }, @"
                SELECT [StatusId]
                FROM [portal].[Module]
                WHERE [Id] = @Id AND [StatusId] IN @StatusIds;
            ");
        }

        public async Task ChangeStatus(IOperation operation, int moduleId, ModuleStatus status)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = moduleId,
                Status = status,
                StatusName = status.ToString("G")
            }, @"
                UPDATE [portal].[Module]
                SET [StatusId] = @Status
                WHERE [Id] = @Id;

                INSERT INTO [portal].[ModuleHistory] ([ModuleId], [Change])
                VALUES (@Id, 'Module status was changed to ""' + @StatusName + '""');
            ");
        }

        public async Task<ModuleInfoEntity> GetModuleByAlias(IOperation operation, string alias)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<ModuleInfoEntity>(new { Alias = alias }, $@"
                SELECT
                    pm.[Id],
                    pm.[Alias],
                    mc.[Name],
                    mc.[Version],
                    pm.[UserId],
                    au.[FirstName],
                    au.[LastName],
                    pm.[HostId],
                    ph.[Name] AS [HostName],
                    ph.[Domain] AS [HostDomain],
                    ph.[OperatingSystemId] AS [HostOperatingSystem],
                    pm.[StatusId] [Status]
                FROM [portal].[Module] pm
                INNER JOIN [portal].[ActiveModuleConfiguration] amc ON amc.[ModuleId] = pm.[Id]
                INNER JOIN [portal].[ModuleConfiguration] mc ON mc.[Id] = amc.[ModuleConfigurationId]
                INNER JOIN [authentication].[User] au ON au.[Id] = pm.[UserId]
                INNER JOIN [portal].[Host] ph ON ph.[Id] = pm.[HostId]
                WHERE pm.[Alias] = @Alias;
            ");
        }

        public async Task<int> CreateModule(IOperation operation, int userId, int hostId, int fileId, ModuleConfigurationFile configuration)
        {
            return await operation.Connection.QuerySingleOrDefaultAsync<int>(new
            {
                UserId = userId,
                HostId = hostId,
                FileId = fileId,
                Alias = configuration.Alias.ToUpper(),
                configuration.Name,
                configuration.Version,
                configuration.Author,
                configuration.Email
            }, @"
                DECLARE @Ids TABLE ([Id] INT);

                INSERT INTO [portal].[Module] ([Alias], [UserId], [HostId])
                OUTPUT INSERTED.[Id] INTO @Ids
                VALUES (@Alias, @UserId, @HostId);

                DECLARE @Id INT;
                SELECT @Id = [Id] FROM @Ids;

                DECLARE @ConfigurationIds TABLE ([Id] INT);

                INSERT INTO [portal].[ModuleConfiguration] ([ModuleId], [Name], [Version], [Author], [Email], [FileId])
                OUTPUT INSERTED.[Id] INTO @ConfigurationIds
                VALUES (@Id, @Name, @Version, @Author, @Email, @FileId);

                DECLARE @ConfigurationId INT;
                SELECT @ConfigurationId = [Id] FROM @ConfigurationIds;

                INSERT INTO [portal].[ActiveModuleConfiguration] ([ModuleId], [ModuleConfigurationId])
                VALUES (@Id, @ConfigurationId);

                INSERT INTO [portal].[ModuleHistory] ([ModuleId], [Change])
                VALUES (@Id, 'Module was created');

                SELECT @Id;
            ");
        }

        public async Task UpdateModule(IOperation operation, int moduleId, int userId)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = moduleId,
                UserId = userId
            }, @"
                UPDATE [portal].[Module]
                SET [UserId] = @UserId
                WHERE [Id] = @Id;

                INSERT INTO [portal].[ModuleHistory] ([ModuleId], [Change])
                VALUES (@Id, 'Module was updated');
            ");
        }

        public async Task UpgradeModule(IOperation operation, int moduleId, int userId, int fileId, ModuleConfigurationFile configuration)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = moduleId,
                UserId = userId,
                FileId = fileId,
                configuration.Name,
                configuration.Version,
                configuration.Author,
                configuration.Email
            }, @"
                UPDATE [portal].[Module]
                SET
                    [UserId] = @UserId,
                    [StatusId] = 5 -- Status is 'Updated To Upgrade'
                WHERE [Id] = @Id;

                DECLARE @ConfigurationIds TABLE ([Id] INT);

                INSERT INTO [portal].[ModuleConfiguration] ([ModuleId], [Name], [Version], [Author], [Email], [FileId])
                OUTPUT INSERTED.[Id] INTO @ConfigurationIds
                VALUES (@Id, @Name, @Version, @Author, @Email, @FileId);

                DECLARE @ConfigurationId INT;
                SELECT @ConfigurationId = [Id] FROM @ConfigurationIds;

                UPDATE [portal].[ActiveModuleConfiguration]
                SET [ModuleConfigurationId] = @ConfigurationId
                WHERE [ModuleId] = @Id;

                INSERT INTO [portal].[ModuleHistory] ([ModuleId], [Change])
                VALUES (@Id, 'Module was prepared to upgrade');
            ");
        }

        public async Task UpgradeModule(IOperation operation, int moduleId, int userId, int configurationId)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = moduleId,
                UserId = userId,
                ConfigurationId = configurationId
            }, @"
                UPDATE [portal].[Module]
                SET
                    [UserId] = @UserId,
                    [StatusId] = 5 -- Status is 'Updated To Upgrade'
                WHERE [Id] = @Id;

                UPDATE [portal].[ActiveModuleConfiguration]
                SET [ModuleConfigurationId] = @ConfigurationId
                WHERE [ModuleId] = @Id;

                INSERT INTO [portal].[ModuleHistory] ([ModuleId], [Change])
                VALUES (@Id, 'Module was prepared to upgrade');
            ");
        }

        public async Task DowngradeModule(IOperation operation, int moduleId, int userId, int configurationId)
        {
            await operation.Connection.ExecuteAsync(new
            {
                Id = moduleId,
                UserId = userId,
                ConfigurationId = configurationId
            }, @"
                UPDATE [portal].[Module]
                SET
                    [UserId] = @UserId,
                    [StatusId] = 9 -- Status is 'Updated To Downgrade'
                WHERE [Id] = @Id;

                UPDATE [portal].[ActiveModuleConfiguration]
                SET [ModuleConfigurationId] = @ConfigurationId
                WHERE [ModuleId] = @Id;

                INSERT INTO [portal].[ModuleHistory] ([ModuleId], [Change])
                VALUES (@Id, 'Module was prepared to downgrade');
            ");
        }

        public async Task DeleteModule(IOperation operation, int moduleId)
        {
            await operation.Connection.ExecuteAsync(new { Id = moduleId }, @"
                DELETE FROM [portal].[Module]
                WHERE [Id] = @Id;
            ");
        }

        public async Task<IEnumerable<ModuleHistoryEntity>> GetModuleHistory(IOperation operation, int moduleId)
        {
            return await operation.Connection.QueryAsync<ModuleHistoryEntity>(new { Id = moduleId }, @"
                SELECT [Time], [Change]
                FROM [portal].[ModuleHistory]
                WHERE [ModuleId] = @Id;
            ");
        }
    }
}