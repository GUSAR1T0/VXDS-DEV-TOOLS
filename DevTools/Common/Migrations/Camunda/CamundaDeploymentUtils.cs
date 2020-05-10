using System;
using System.Reflection;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using VXDesign.Store.DevTools.Common.Clients.Camunda;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Core.Properties;
using VXDesign.Store.DevTools.Common.Migrations.Common;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Migrations.Camunda
{
    public static class CamundaDeploymentUtils
    {
        private const string Scope = "VXDS_CAM";

        public static void Perform(string[] args, DatabaseConnectionProperties databaseProperties, SyrinxProperties syrinxProperties, Assembly assembly,
            Action<IServiceCollection> serviceHandler = null)
        {
            var parser = new Parser(settings => settings.CaseInsensitiveEnumValues = true);
            parser.ParseArguments<CamundaDeploymentToolOptions>(args)
                .WithParsed(options =>
                {
                    var serviceCollection = CreateServices(databaseProperties, syrinxProperties, assembly);
                    serviceHandler?.Invoke(serviceCollection);
                    using var scope = serviceCollection.BuildServiceProvider(false).CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<ICamundaDeploymentService>();

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
                            throw CommonExceptions.CamundaCouldNotBeDeployed();
                    }
                })
                .WithNotParsed(_ => throw CommonExceptions.CamundaCouldNotBeDeployed());
        }

        private static IServiceCollection CreateServices(DatabaseConnectionProperties databaseProperties, SyrinxProperties syrinxProperties, Assembly assembly) => new ServiceCollection()
            .AddSingleton<ILoggerStore>(factory => new LoggerStore(databaseProperties.LogStoreConnectionString, Scope))
            .AddSingleton(factory => new CamundaDeploymentParameters
            {
                Assembly = assembly
            })
            .AddScoped<IOperationService>(factory =>
            {
                var loggerStore = factory.GetService<ILoggerStore>();
                var dataStoreConnectionString = databaseProperties.DataStoreConnectionString;
                return new OperationService(loggerStore, dataStoreConnectionString, Scope);
            })
            .AddScoped<ISyrinxCamundaClientService>(factory => new SyrinxCamundaClientService(syrinxProperties))
            .AddScoped<ICamundaDeploymentService, CamundaDeploymentService>();
    }
}