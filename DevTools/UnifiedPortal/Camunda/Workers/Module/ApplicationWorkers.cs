using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public static class Application
    {
        [CamundaWorkerTopic("UnifiedPortal.Application.Launch")]
        public class LaunchWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Application.Stop")]
        public class StopWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }
    }
}