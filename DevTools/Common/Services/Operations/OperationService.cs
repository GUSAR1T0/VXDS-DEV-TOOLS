using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;
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
        private const string OperationErrorMessage = "Failed to perform action correctly";
        private const string TransactionRollbackErrorMessage = "Failed to rollback transaction";
        private const string SuccessMessage = "Action was performed correctly";

        private readonly DatabaseConnectionProperties properties;
        private readonly string scope;

        public OperationService(DatabaseConnectionProperties properties, string scope)
        {
            this.properties = properties;
            this.scope = scope;
        }

        private static dynamic GetExceptionContent(Exception exception) => new
        {
            Type = exception.GetType().FullName,
            exception.Source,
            Data = JsonConvert.SerializeObject(exception.Data),
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
                var isSuccessful = true;
                var logger = operation.Logger<OperationService>();
                try
                {
                    using (var transaction = ((OperationConnection) operation.Connection).BeginTransaction())
                    {
                        try
                        {
                            var result = await action(operation);
                            transaction.Commit();
                            await logger.Debug(SuccessMessage);
                            return result;
                        }
                        catch (Exception actionException)
                        {
                            isSuccessful = false;

                            var retries = 5;
                            while (retries > 0)
                            {
                                try
                                {
                                    transaction.Rollback();
                                    break;
                                }
                                catch (Exception rollbackException)
                                {
                                    retries--;
                                    await logger.Error($"{TransactionRollbackErrorMessage}, remaining attempts: {retries}", GetExceptionContent(rollbackException));
                                    await Task.Delay(TimeSpan.FromSeconds(1));
                                }
                            }

                            if (retries == 0)
                            {
                                await logger.Error($"{TransactionRollbackErrorMessage}, operation couldn't be aborted");
                            }

                            await logger.Error(OperationErrorMessage, GetExceptionContent(actionException));
                            throw;
                        }
                    }
                }
                finally
                {
                    ((OperationConnection) operation.Connection).EndTransaction();
                    await operation.Complete(isSuccessful);
                }
            }
        }
    }
}