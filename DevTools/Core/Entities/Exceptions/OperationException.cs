using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using VXDesign.Store.DevTools.Core.Entities.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Exceptions
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