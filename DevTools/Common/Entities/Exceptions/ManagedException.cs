using System;
using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class ManagedException : Exception
    {
        public string OperationId { get; }

        public ManagedException(IOperation operation, string message) : base(message)
        {
            OperationId = operation.OperationId;
        }
    }
}