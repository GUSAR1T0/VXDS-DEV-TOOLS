using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Storage.LogStores;

namespace VXDesign.Store.DevTools.Core.Services.Operations
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

        private readonly ILoggerStore loggerStore;
        private readonly string dataStoreConnectionString;
        private readonly string scope;

        public OperationService(ILoggerStore loggerStore, string dataStoreConnectionString, string scope)
        {
            this.loggerStore = loggerStore;
            this.dataStoreConnectionString = dataStoreConnectionString;
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
            context.Scope = scope;
            using var operation = new Operation(loggerStore, dataStoreConnectionString, context);
            var isSuccessful = true;
            var logger = operation.Logger<OperationService>();
            try
            {
                await using var transaction = ((OperationConnection) operation.Connection).BeginTransaction();
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

                    switch (actionException)
                    {
                        case AuthenticationException _:
                        case BadRequestException _:
                        case NotFoundException _:
                        case OperationException _:
                            throw;
                        default:
                            throw new OperationException(operation, OperationErrorMessage, actionException);
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