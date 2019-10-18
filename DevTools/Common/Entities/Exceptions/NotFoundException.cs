using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Exceptions
{
    public class NotFoundException : ManagedException
    {
        public NotFoundException(IOperation operation, string message) : base(operation, message)
        {
        }
    }
}