using System;
using System.Reflection;
using CommandLine;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.Common.Migrations.Common;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Migrations.Database
{
    public static class DatabaseMigrationUtils
    {
        private const string Scope = "VXDS_DB";

        public static void Perform(string[] args, DatabaseConnectionProperties properties, Assembly assembly, Action<IServiceCollection> serviceHandler = null)
        {
            var parser = new Parser(settings => settings.CaseInsensitiveEnumValues = true);
            parser.ParseArguments<DatabaseMigrationToolOptions>(args)
                .WithParsed(options =>
                {
                    var serviceCollection = CreateServices(properties, assembly);
                    serviceHandler?.Invoke(serviceCollection);
                    using var scope = serviceCollection.BuildServiceProvider(false).CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();

                    switch (options.Action)
                    {
                        case MigrationAction.Upgrade:
                            service.Upgrade();
                            break;
                        case MigrationAction.Previous:
                            service.DowngradeToPrevious();
                            break;
                        case MigrationAction.Downgrade:
                            service.Downgrade();
                            break;
                        default:
                            throw CommonExceptions.DatabaseCouldNotBeMigrated();
                    }
                })
                .WithNotParsed(_ => throw CommonExceptions.DatabaseCouldNotBeMigrated());
        }

        private static IServiceCollection CreateServices(DatabaseConnectionProperties properties, Assembly assembly) => new ServiceCollection()
            .AddFluentMigratorCore()
            .AddSingleton<ILoggerStore>(factory => new LoggerStore(properties.LogStoreConnectionString, Scope))
            .AddScoped<IOperationService>(factory =>
            {
                var loggerStore = factory.GetService<ILoggerStore>();
                var dataStoreConnectionString = properties.DataStoreConnectionString;
                return new OperationService(loggerStore, dataStoreConnectionString, Scope);
            })
            .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(properties.DataStoreConnectionString)
                .ScanIn(assembly)
                .For.VersionTableMetaData()
                .For.EmbeddedResources()
                .For.Migrations()
            )
            .AddScoped<IDatabaseMigrationService, DatabaseMigrationService>()
            .AddLogging(lb => lb.AddNLog());
    }
}