using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers
{
    [CamundaWorkerTopic("UnifiedPortal.Database.Upgrade")]
    public class DatabaseMigrationUpgradeWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Database.Previous")]
    public class DatabaseMigrationDowngradeToPreviousWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Database.Downgrade")]
    public class DatabaseMigrationDowngradeWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }
}