using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Core.Operations
{
    public interface IOperationService
    {
        Task Make(OperationContext context, Func<IOperation, Task> action);
        Task<T> Make<T>(OperationContext context, Func<IOperation, Task<T>> action);
    }

    public class OperationService : IOperationService
    {
        private readonly ILoggerStore loggerStore;
        private readonly string dataStoreConnectionString;
        private readonly string scope;

        public OperationService(ILoggerStore loggerStore, string dataStoreConnectionString, string scope)
        {
            this.loggerStore = loggerStore;
            this.dataStoreConnectionString = dataStoreConnectionString;
            this.scope = scope;
        }

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
                await using var transaction = ((OperationConnection) operation.Connection).BeginTransaction(context.IsolationLevel);
                try
                {
                    var result = await action(operation);
                    transaction.Commit();
                    await logger.Debug(OperationLogMessage.SuccessMessage);
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
                            await logger.Error($"{OperationLogMessage.TransactionRollbackErrorMessage}, remaining attempts: {retries}", rollbackException.ToLog());
                            if (retries != 0) await Task.Delay(TimeSpan.FromSeconds(1));
                        }
                    }

                    if (retries == 0)
                    {
                        await logger.Error($"{OperationLogMessage.TransactionRollbackErrorMessage}, operation couldn't be aborted");
                    }

                    await logger.Error(OperationLogMessage.OperationErrorMessage, actionException.ToLog());

                    switch (actionException)
                    {
                        case AuthenticationException _:
                        case BadRequestException _:
                        case NotFoundException _:
                        case OperationException _:
                            throw;
                        default:
                            throw new OperationException(operation, OperationLogMessage.OperationErrorMessage, actionException);
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