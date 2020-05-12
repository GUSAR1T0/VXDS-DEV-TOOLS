using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public static class DatabaseMigration
    {
        [CamundaWorkerTopic("UnifiedPortal.Database.Upgrade")]
        public class UpgradeWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Database.Rollback")]
        public class RollbackWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Database.Downgrade")]
        public class DowngradeWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }
    }
}