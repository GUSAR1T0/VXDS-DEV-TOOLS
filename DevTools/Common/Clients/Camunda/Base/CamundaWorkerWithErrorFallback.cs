using System;
using VXDesign.Store.DevTools.Common.Clients.Camunda.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Base
{
    public abstract class CamundaWorkerWithErrorFallback : CamundaWorker
    {
        public abstract string ErrorCode { get; }

        public override void Execute(IOperation operation, IOperationLogger logger)
        {
            try
            {
                ExecuteWithFallback(operation, logger);
            }
            catch (Exception e)
            {
                throw new CamundaWorkerBpmnError(e.Message, ErrorCode);
            }
        }

        protected abstract void ExecuteWithFallback(IOperation operation, IOperationLogger logger);
    }
}