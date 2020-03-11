using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Core.Exceptions
{
    public class BadRequestException : OperationException
    {
        public BadRequestException(IOperation operation, string message) : base(operation, message)
        {
        }
    }
}