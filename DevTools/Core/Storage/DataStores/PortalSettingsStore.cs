using System.Collections.Generic;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.Settings;

namespace VXDesign.Store.DevTools.Core.Storage.DataStores
{
    public interface IPortalSettingsStore
    {
        Task<IEnumerable<SettingsParametersItemEntity>> GetSettingsParameters(IOperation operation);
        Task<string> GetSettingsParameter(IOperation operation, string name);
        Task ModifySettings(IOperation operation, string name, string value);
    }

    public class PortalSettingsStore : BaseDataStore, IPortalSettingsStore
    {
        public async Task<IEnumerable<SettingsParametersItemEntity>> GetSettingsParameters(IOperation operation)
        {
            return await operation.Connection.QueryAsync<SettingsParametersItemEntity>(@"
                SELECT
                    [Name],
                    [Value]
                FROM [portal].[Settings];
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
    }
}