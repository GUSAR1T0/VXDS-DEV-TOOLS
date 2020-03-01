using VXDesign.Store.DevTools.Common.Core.Operations;

namespace VXDesign.Store.DevTools.Common.Core.Exceptions
{
    public class NotFoundException : OperationException
    {
        public NotFoundException(IOperation operation, string message) : base(operation, message)
        {
        }
    }
}