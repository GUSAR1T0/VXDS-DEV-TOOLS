using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using VXDesign.Store.DevTools.Core.Storage.LogStores;
using VXDesign.Store.DevTools.Core.Utils.Base;
using VXDesign.Store.DevTools.Database;

namespace VXDesign.Store.DevTools.UnifiedPortal.Database
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ConfigurationUtils.GetEnvironmentConfiguration();
            ConsoleApplicationLauncher.Launch(() =>
            {
                var serviceProvider = CreateServices(configuration["Database:DataStoreConnectionString"], configuration["Database:LogStoreConnectionString"]);
                using var scope = serviceProvider.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                if (args.Length >= 1)
                {
                    var manipulation = (DatabaseMigrationsAction) Enum.Parse(typeof(DatabaseMigrationsAction), args[0], true);
                    var version = args.Length > 1 && long.TryParse(args[1], out var value) ? value : (long?) null;

                    switch (manipulation)
                    {
                        case DatabaseMigrationsAction.Down:
                            DowngradeDatabase(runner, version);
                            break;
                        case DatabaseMigrationsAction.Up:
                            UpgradeDatabase(runner, version);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    UpgradeDatabase(runner);
                }
            });
        }

        private static IServiceProvider CreateServices(string dataStoreConnectionString, string logStoreConnectionString) => new ServiceCollection()
            .AddFluentMigratorCore()
            .AddSingleton<ILoggerStore>(factory => new LoggerStore(logStoreConnectionString, "VXDS_DB"))
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(dataStoreConnectionString)
                .ScanIn(typeof(Program).Assembly)
                .For.EmbeddedResources()
                .For.Migrations())
            .AddLogging(lb => lb.AddNLog())
            .BuildServiceProvider(false);

        private static void UpgradeDatabase(IMigrationRunner runner, long? version = null)
        {
            switch (version)
            {
                case null:
                    runner.MigrateUp();
                    break;
                default:
                    runner.MigrateUp(version.Value);
                    break;
            }
        }

        private static void DowngradeDatabase(IMigrationRunner runner, long? version = null) => runner.MigrateDown(version ?? 0);
    }
}