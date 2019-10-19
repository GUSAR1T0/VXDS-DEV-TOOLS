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
        int OperationId { get; }
        bool? IsSuccessful { get; }

        IOperationConnection Connection { get; }
        IOperationLogger Logger<T>();
    }

    public class Operation : IOperation
    {
        private readonly ILoggerStore loggerStore;
        private readonly IOperationStore operationStore;

        public int OperationId { get; }
        public bool? IsSuccessful { get; private set; }

        public IOperationConnection Connection { get; }

        internal Operation(string scope, OperationContext context, DatabaseConnectionProperties properties)
        {
            loggerStore = new LoggerStore(properties.LogStoreConnectionString, scope);

            Connection = new OperationConnection(properties.DataStoreConnectionString);
            operationStore = new OperationStore(Connection);
            OperationId = operationStore.Start(scope, context).Result;
        }

        public IOperationLogger Logger<T>() => new OperationLogger<T>(loggerStore, OperationId);

        public async Task Complete(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
            await operationStore.Stop(OperationId, isSuccessful);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}