using System;
using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Core.Migrations;
using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.Common.Storage.LogStorage;

namespace VXDesign.Store.DevTools.Common.Core.Utils
{
    public static class MigrationUtils
    {
        public static void Perform(string[] args, DatabaseConnectionProperties properties, Assembly assembly)
        {
            var serviceProvider = CreateServices(properties, assembly);
            using var scope = serviceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

            if (args.Length >= 1)
            {
                var manipulation = (MigrationAction) Enum.Parse(typeof(MigrationAction), args[0], true);
                var version = args.Length > 1 && long.TryParse(args[1], out var value) ? value : (long?) null;
 
                switch (manipulation)
                {
                    case MigrationAction.Up:
                        runner.UpgradeDatabase(version);
                        break;
                    case MigrationAction.Down:
                        runner.DowngradeDatabase(version);
                        break;
                    default:
                        throw CommonExceptions.DatabaseCouldNotBeMigrated();
                }
            }
            else
            {
                runner.UpgradeDatabase();
            }
        }

        private static IServiceProvider CreateServices(DatabaseConnectionProperties properties, Assembly assembly) => new ServiceCollection()
            .AddFluentMigratorCore()
            .AddSingleton<ILoggerStore>(factory => new LoggerStore(properties.LogStoreConnectionString, "VXDS_DB"))
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(properties.DataStoreConnectionString)
                .ScanIn(assembly)
                .For.VersionTableMetaData()
                .For.EmbeddedResources()
                .For.Migrations())
            .AddLogging(lb => lb.AddNLog())
            .BuildServiceProvider(false);
    }
}