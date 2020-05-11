using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers
{
    [CamundaWorkerTopic("UnifiedPortal.Application.Launch")]
    public class ApplicationLaunchWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Application.Stop")]
    public class ApplicationStopWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }
}