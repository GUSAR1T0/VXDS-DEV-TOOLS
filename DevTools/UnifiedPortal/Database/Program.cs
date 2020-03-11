using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.Common.Core.Utils;

namespace VXDesign.Store.DevTools.UnifiedPortal.Database
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ConfigurationUtils.GetEnvironmentConfiguration();
            ConsoleApplicationUtils.Launch(() => MigrationUtils.Perform(args, new DatabaseConnectionProperties
            {
                DataStoreConnectionString = configuration["Database:DataStoreConnectionString"],
                LogStoreConnectionString = configuration["Database:LogStoreConnectionString"]
            }, typeof(Program).Assembly));
        }
    }
}