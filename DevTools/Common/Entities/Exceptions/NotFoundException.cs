using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class NotFoundException : OperationException
    {
        public NotFoundException(IOperation operation, string message) : base(operation, message)
        {
        }
    }
}