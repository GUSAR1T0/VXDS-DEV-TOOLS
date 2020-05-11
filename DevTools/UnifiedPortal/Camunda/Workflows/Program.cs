using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Migrations.Camunda;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workflows
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ConfigurationUtils.GetEnvironmentConfiguration();
            ConsoleApplicationUtils.Launch(() => CamundaDeploymentUtils.Perform(args, new DatabaseConnectionProperties
            {
                DataStoreConnectionString = configuration["Database:DataStoreConnectionString"],
                LogStoreConnectionString = configuration["Database:LogStoreConnectionString"]
            }, new SyrinxProperties
            {
                Host = configuration["Syrinx:Host"]
            }, typeof(Program).Assembly));
        }
    }
}