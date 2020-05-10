using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Migrations.Database;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Database
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = ConfigurationUtils.GetEnvironmentConfiguration();
            ConsoleApplicationUtils.Launch(() => DatabaseMigrationUtils.Perform(args, new DatabaseConnectionProperties
            {
                DataStoreConnectionString = configuration["Database:DataStoreConnectionString"],
                LogStoreConnectionString = configuration["Database:LogStoreConnectionString"]
            }, typeof(Program).Assembly));
        }
    }
}