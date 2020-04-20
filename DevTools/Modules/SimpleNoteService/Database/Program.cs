using Microsoft.Extensions.DependencyInjection;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Database
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = ConfigurationUtils.GetEnvironmentConfiguration();
            ConsoleApplicationUtils.Launch(() => MigrationUtils.Perform(args, new DatabaseConnectionProperties
            {
                DataStoreConnectionString = configuration["Database:DataStoreConnectionString"],
                LogStoreConnectionString = configuration["Database:LogStoreConnectionString"]
            }, typeof(Program).Assembly, services =>
            {
                services.AddScoped<IOperationService>(factory =>
                {
                    var loggerStore = factory.GetService<ILoggerStore>();
                    var dataStoreConnectionString = configuration["Database:DataStoreConnectionString"];
                    return new OperationService(loggerStore, dataStoreConnectionString, "VXDS_DB");
                });
            }));
        }
    }
}