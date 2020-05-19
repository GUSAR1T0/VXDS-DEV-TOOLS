using System;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Common.Core.Operations
{
    public interface IOperation : IDisposable
    {
        OperationContext OperationContext { get; }
        long OperationId { get; }
        string ComplexOperationId { get; }
        bool? IsSuccessful { get; }

        IOperationConnection Connection { get; }
        IOperationLogger Logger<T>();
    }

    public class Operation : IOperation
    {
        private readonly ILoggerStore loggerStore;
        private readonly IOperationManagerStore operationManagerStore;
        private readonly long? operationId;

        public OperationContext OperationContext { get; }
        public long OperationId => operationId ?? -1; // -1: when operation is ready to be started but the record isn't stored into DB
        public string ComplexOperationId => $"{(OperationContext.ParentOperation != null ? $"{OperationContext.ParentOperation.OperationId} -> " : "")}{OperationId}";
        public bool? IsSuccessful { get; private set; }

        public IOperationConnection Connection { get; }

        internal Operation(ILoggerStore loggerStore, string dataStoreConnectionString, OperationContext context)
        {
            this.loggerStore = loggerStore;
            OperationContext = context;

            Connection = new OperationConnection(this, dataStoreConnectionString);
            operationManagerStore = new OperationManagerStore(Connection);
            operationId = operationManagerStore.Start(context).Result;
        }

        public IOperationLogger Logger<T>() => new OperationLogger<T>(loggerStore, OperationId);

        public async Task Complete(bool isSuccessful)
        {
            await operationManagerStore.Stop(OperationId, isSuccessful);
            IsSuccessful = isSuccessful;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}