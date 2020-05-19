using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.Constants;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Host
{
    public abstract class HostProcessingWorker : CamundaWorker
    {
        [CamundaWorkerVariable(Name = CamundaWorkerKey.HostId, Direction = CamundaVariableDirection.Input)]
        public int HostId { get; set; }
    }
}