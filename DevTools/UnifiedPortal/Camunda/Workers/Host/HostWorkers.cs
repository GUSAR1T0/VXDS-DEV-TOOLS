using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.DataStorage.Stores;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Host
{
    public static class Host
    {
        [CamundaWorkerTopic("UnifiedPortal.Host.Delete")]
        public class DeleteWorker : HostProcessingWorker
        {
            private readonly IPortalSettingsStore portalSettingsStore;

            public DeleteWorker(IPortalSettingsStore portalSettingsStore)
            {
                this.portalSettingsStore = portalSettingsStore;
            }

            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                portalSettingsStore.DeleteHost(operation, HostId).Wait();
            }
        }
    }
}