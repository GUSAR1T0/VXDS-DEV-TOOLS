using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Endpoints;
using VXDesign.Store.DevTools.Common.Core.Constants;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public abstract class ModuleProcessingWorker : CamundaWorkerWithErrorFallback
    {
        public override string ErrorCode => CamundaWorkerKey.ModuleProcessingError;

        [CamundaWorkerVariable(Name = CamundaWorkerKey.ModuleId, Direction = CamundaVariableDirection.Input)]
        public int ModuleId { get; set; }
    }
}