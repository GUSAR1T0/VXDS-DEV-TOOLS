using System;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Properties;
using VXDesign.Store.DevTools.Common.Storage.DataStores;
using VXDesign.Store.DevTools.Common.Storage.LogStores;
using IOperationStore = VXDesign.Store.DevTools.Common.Storage.DataStores.IOperationStore;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public interface IOperation : IDisposable
    {
        OperationContext OperationContext { get; }
        long OperationId { get; }
        bool? IsSuccessful { get; }

        IOperationConnection Connection { get; }
        IOperationLogger Logger<T>();
    }

    public class Operation : IOperation
    {
        private readonly ILoggerStore loggerStore;
        private readonly IOperationStore operationStore;
        private readonly long? operationId;

        public OperationContext OperationContext { get; }
        public long OperationId => operationId ?? -1; // -1: when operation is ready to be started but the record isn't stored into DB
        public bool? IsSuccessful { get; private set; }

        public IOperationConnection Connection { get; }

        internal Operation(string scope, OperationContext context, DatabaseConnectionProperties properties)
        {
            OperationContext = context;
            loggerStore = new LoggerStore(properties.LogStoreConnectionString, scope);

            Connection = new OperationConnection(this, properties.DataStoreConnectionString);
            operationStore = new OperationStore(Connection);
            operationId = operationStore.Start(scope, context).Result;
        }

        public IOperationLogger Logger<T>() => new OperationLogger<T>(loggerStore, OperationId);

        public async Task Complete(bool isSuccessful)
        {
            await operationStore.Stop(OperationId, isSuccessful);
            IsSuccessful = isSuccessful;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}