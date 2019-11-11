using VXDesign.Store.DevTools.Core.Entities.Operations;

namespace VXDesign.Store.DevTools.Core.Entities.Exceptions
{
    public class NotFoundException : OperationException
    {
        public NotFoundException(IOperation operation, string message) : base(operation, message)
        {
        }
    }
}