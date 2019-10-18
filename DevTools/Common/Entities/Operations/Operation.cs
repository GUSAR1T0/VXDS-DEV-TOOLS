using System;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Storage.LogStores;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public interface IOperation : IDisposable
    {
        bool? IsSuccess { get; set; }

        IOperationConnection Connection { get; }
        IOperationLogger Logger<T>();
    }

    public class Operation : IOperation
    {
        private readonly ILoggerStore loggerStore;
        private readonly IOperationStore operationStore;

        private string OperationId { get; }
        public bool? IsSuccess { get; set; }
        public IOperationConnection Connection { get; }

        public Operation(int userId, OperationContext context, DatabaseConnectionProperties properties)
        {
            loggerStore = new LoggerStore(properties.LogStoreConnectionString);
            operationStore = new OperationStore(properties.LogStoreConnectionString);

            OperationId = operationStore.Start(userId, context).Result;
            Connection = new OperationConnection(OperationId, properties.DataStoreConnectionString);
        }

        public IOperationLogger Logger<T>() => new OperationLogger<T>(loggerStore, OperationId);

        public void Dispose()
        {
            operationStore.Stop(OperationId, IsSuccess ?? true);
            Connection.Dispose();
        }
    }
}