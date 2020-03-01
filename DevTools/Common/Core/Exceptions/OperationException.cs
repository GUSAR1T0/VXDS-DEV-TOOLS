using System;
using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Core.Exceptions
{
    public class OperationException : Exception
    {
        public long OperationId { get; }

        public OperationException(IOperation operation, string message, Exception exception = null) : base(GetFormattedErrorMessage(message, exception))
        {
            OperationId = operation.OperationId;
        }

        private static string GetFormattedErrorMessage(string message, Exception exception = null)
        {
            var error = message;
            if (exception != null) error += $": \"{exception.Message}\"";
            return error;
        }
    }
}