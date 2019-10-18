using System;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Services.Operations
{
    public interface IOperationService
    {
        Task Make(int userId, OperationContext context, Func<IOperation, Task> action);
        Task<T> Make<T>(int userId, OperationContext context, Func<IOperation, Task<T>> action);
    }

    public class OperationService : IOperationService
    {
        private const string ErrorMessage = "Failed to perform action correctly";

        private readonly DatabaseConnectionProperties properties;

        public OperationService(DatabaseConnectionProperties properties)
        {
            this.properties = properties;
        }

        private static object GetExceptionContent(Exception exception) => new
        {
            Type = exception.GetType().FullName,
            exception.Source,
            exception.Data,
            exception.Message,
            exception.StackTrace
        };

        public async Task Make(int userId, OperationContext context, Func<IOperation, Task> action)
        {
            using (var operation = new Operation(userId, context, properties))
            {
                try
                {
                    await action(operation);
                }
                catch (Exception e)
                {
                    operation.IsSuccess = false;
                    await operation.Logger<OperationService>().Error(ErrorMessage, GetExceptionContent(e));
                    throw;
                }
            }
        }

        public async Task<T> Make<T>(int userId, OperationContext context, Func<IOperation, Task<T>> action)
        {
            using (var operation = new Operation(userId, context, properties))
            {
                try
                {
                    return await action(operation);
                }
                catch (Exception e)
                {
                    operation.IsSuccess = false;
                    await operation.Logger<OperationService>().Error(ErrorMessage, GetExceptionContent(e));
                    throw;
                }
            }
        }
    }
}