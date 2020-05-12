using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public static class CamundaDeployment
    {
        [CamundaWorkerTopic("UnifiedPortal.Camunda.Upgrade")]
        public class UpgradeWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Camunda.Rollback")]
        public class RollbackWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Camunda.Downgrade")]
        public class DowngradeWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Camunda.Launch")]
        public class LaunchWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Camunda.Stop")]
        public class StopWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }
    }
}