using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers
{
    [CamundaWorkerTopic("UnifiedPortal.Fixture.Before")]
    public class FixtureBeforeWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Fixture.After")]
    public class FixtureAfterWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }

    [CamundaWorkerTopic("UnifiedPortal.Fixture.Error")]
    public class FixtureErrorWorker : CamundaWorker
    {
        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            logger.Info("PASS").Wait();
        }
    }
}