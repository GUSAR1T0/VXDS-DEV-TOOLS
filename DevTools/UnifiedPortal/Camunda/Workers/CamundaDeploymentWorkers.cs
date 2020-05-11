using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers
{
    [CamundaWorkerTopic("UnifiedPortal.Camunda.Upgrade")]
    public class CamundaDeploymentUpgradeWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Camunda.Previous")]
    public class CamundaDeploymentPreviousWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Camunda.Downgrade")]
    public class CamundaDeploymentDowngradeWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Camunda.Launch")]
    public class CamundaDeploymentLaunchWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Camunda.Stop")]
    public class CamundaDeploymentStopWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }
}