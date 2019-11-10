using VXDesign.Store.DevTools.Core.Entities.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Exceptions
{
    public class BadRequestException : OperationException
    {
        public BadRequestException(IOperation operation, string message) : base(operation, message)
        {
        }
    }
}