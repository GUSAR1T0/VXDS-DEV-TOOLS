using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class AuthenticationException : OperationException
    {
        public int StatusCode { get; }

        public AuthenticationException(IOperation operation, string message, int statusCode) : base(operation, message)
        {
            StatusCode = statusCode;
        }
    }
}