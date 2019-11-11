using System;
using VXDesign.Store.DevTools.Core.Entities.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Exceptions
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