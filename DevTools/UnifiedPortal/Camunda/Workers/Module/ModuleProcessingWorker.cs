using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public abstract class ModuleProcessingWorker : CamundaWorkerWithErrorFallback
    {
        public override string ErrorCode => "module-processing-error";
    }
}