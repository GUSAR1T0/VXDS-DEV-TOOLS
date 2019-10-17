using System;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Services.Operations
{
    public interface IOperationService
    {
        Task Make(Func<IOperation, Task> action);
        Task<T> Make<T>(Func<IOperation, Task<T>> action);
    }

    public class OperationService : IOperationService
    {
        private readonly DatabaseConnectionProperties properties;

        public OperationService(DatabaseConnectionProperties properties)
        {
            this.properties = properties;
        }

        public async Task Make(Func<IOperation, Task> action)
        {
            using (var operation = new Operation(properties))
            {
                await action(operation);
            }
        }

        public async Task<T> Make<T>(Func<IOperation, Task<T>> action)
        {
            using (var operation = new Operation(properties))
            {
                return await action(operation);
            }
        }
    }
}