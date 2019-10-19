using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class BadRequestException : OperationException
    {
        public BadRequestException(IOperation operation, string message) : base(operation, message)
        {
        }
    }
}