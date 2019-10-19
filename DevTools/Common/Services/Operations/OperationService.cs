using System;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Services.Operations
{
    public interface IOperationService
    {
        Task Make(OperationContext context, Func<IOperation, Task> action);
        Task<T> Make<T>(OperationContext context, Func<IOperation, Task<T>> action);
    }

    public class OperationService : IOperationService
    {
        private const string ErrorMessage = "Failed to perform action correctly";
        private const string SuccessMessage = "Action was performed correctly";

        private readonly DatabaseConnectionProperties properties;
        private readonly string scope;

        public OperationService(DatabaseConnectionProperties properties, string scope)
        {
            this.properties = properties;
            this.scope = scope;
        }

        private static object GetExceptionContent(Exception exception) => new
        {
            Type = exception.GetType().FullName,
            exception.Source,
            exception.Data,
            exception.Message,
            exception.StackTrace
        };

        public async Task Make(OperationContext context, Func<IOperation, Task> action) => await Make(context, async operation =>
        {
            await action(operation);
            return true;
        });

        public async Task<T> Make<T>(OperationContext context, Func<IOperation, Task<T>> action)
        {
            using (var operation = new Operation(scope, context, properties))
            {
                var isSuccess = true;
                var logger = operation.Logger<OperationService>();
                try
                {
                    var result = await action(operation);
                    await logger.Debug(SuccessMessage);
                    return result;
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    await logger.Error(ErrorMessage, GetExceptionContent(e));
                    throw;
                }
                finally
                {
                    await operation.Complete(isSuccess);
                }
            }
        }
    }
}