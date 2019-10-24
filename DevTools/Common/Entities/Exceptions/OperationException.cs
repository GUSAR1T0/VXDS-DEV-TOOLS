using System;
using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class OperationException : Exception
    {
        public long OperationId { get; }

        public OperationException(IOperation operation, string message) : base(message)
        {
            OperationId = operation.OperationId;
        }
    }
}