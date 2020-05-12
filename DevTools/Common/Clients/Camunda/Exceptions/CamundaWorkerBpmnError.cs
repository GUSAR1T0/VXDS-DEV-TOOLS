using System;

namespace VXDesign.Store.DevTools.Common.Clients.Camunda.Exceptions
{
    public class CamundaWorkerBpmnError : Exception
    {
        public string Code { get; }

        public CamundaWorkerBpmnError(string message, string code) : base(message)
        {
            Code = code;
        }
    }
}