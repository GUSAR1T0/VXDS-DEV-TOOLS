using VXDesign.Store.DevTools.Common.Clients.Camunda.Base;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Camunda.Workers.Module
{
    public static class Fixture
    {
        [CamundaWorkerTopic("UnifiedPortal.Fixture.Before")]
        public class BeforeWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Fixture.After")]
        public class AfterWorker : ModuleProcessingWorker
        {
            protected override void ExecuteWithFallback(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }

        [CamundaWorkerTopic("UnifiedPortal.Fixture.Error")]
        public class ErrorWorker : CamundaWorker
        {
            public override void Execute(IOperation operation, IOperationLogger logger)
            {
                logger.Info("PASS").Wait();
            }
        }
    }
}