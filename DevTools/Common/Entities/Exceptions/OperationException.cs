using System;
using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class OperationException : Exception
    {
        public int OperationId { get; }

        public OperationException(IOperation operation, string message) : base(message)
        {
            OperationId = operation.OperationId;
        }

        public OperationException(IOperation operation, Exception exception) : this(operation, exception.Message)
        {
        }
    }
}