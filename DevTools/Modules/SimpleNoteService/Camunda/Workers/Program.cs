using System;
using Microsoft.Extensions.DependencyInjection;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Utils;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Camunda.Workers.Properties;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Camunda.Workers
{
    class Program
    {
        static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            CamundaWorkers<WorkersProperties>.Builder()
                .SetProperties(() => ConfigurationUtils.GetEnvironmentConfiguration(environment))
                .SetLogger(ConfigurationUtils.GetLoggerConfiguration, "VXDS_CAM_WORK")
                .Configure(collection =>
                {
                    collection.AddScoped<IUserDataStore, UserDataStore>();
                    collection.AddScoped<INoteFolderStore, NoteFolderStore>();
                })
                .AddWorker<Note.Note.NotificationWorker>()
                .Launch();
        }
    }
}