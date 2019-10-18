using System;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Storage.LogStores;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public interface IOperation : IDisposable
    {
        bool? IsSuccess { get; }

        IOperationConnection Connection { get; }
        IOperationLogger Logger<T>();

        Task Complete(bool isSuccess);
    }

    public class Operation : IOperation
    {
        private readonly ILoggerStore loggerStore;
        private readonly IOperationStore operationStore;

        private string OperationId { get; }
        public bool? IsSuccess { get; private set; }
        public IOperationConnection Connection { get; }

        public Operation(string scope, int userId, OperationContext context, DatabaseConnectionProperties properties)
        {
            loggerStore = new LoggerStore(properties.LogStoreConnectionString, scope);
            operationStore = new OperationStore(properties.LogStoreConnectionString, scope);

            OperationId = operationStore.Start(userId, context).Result;
            Connection = new OperationConnection(OperationId, properties.DataStoreConnectionString);
        }

        public IOperationLogger Logger<T>() => new OperationLogger<T>(loggerStore, OperationId);

        public async Task Complete(bool isSuccess)
        {
            IsSuccess = isSuccess;
            await operationStore.Stop(OperationId, isSuccess);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}