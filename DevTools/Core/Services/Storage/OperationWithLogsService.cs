using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.Operation;
using VXDesign.Store.DevTools.Core.Storage.DataStores;
using VXDesign.Store.DevTools.Core.Storage.LogStores;

namespace VXDesign.Store.DevTools.Core.Services.Storage
{
    public interface IOperationWithLogsService
    {
        Task<OperationPagingResponse> GetItems(IOperation operation, OperationPagingRequest request);
    }

    public class OperationWithLogsService : IOperationWithLogsService
    {
        private readonly IOperationStore operationStore;
        private readonly ILoggerStore loggerStore;

        public OperationWithLogsService(IOperationStore operationStore, ILoggerStore loggerStore)
        {
            this.operationStore = operationStore;
            this.loggerStore = loggerStore;
        }

        public async Task<OperationPagingResponse> GetItems(IOperation operation, OperationPagingRequest request)
        {
            var (total, operations) = await operationStore.Get(operation, request);
            var operationList = operations.ToList();
            var logs = await loggerStore.GetByOperations(operationList.Select(item => item.Id));
            return new OperationPagingResponse
            {
                Total = total,
                Items = PreparePagingItems(operationList, logs)
            };
        }

        private static IEnumerable<OperationWithLogs> PreparePagingItems(IEnumerable<OperationEntity> operations, IEnumerable<LogEntity> logs)
        {
            return operations.Select(operation => new OperationWithLogs
            {
                Operation = operation,
                Logs = logs.Where(log => log.OperationId == operation.Id).ToList()
            });
        }
    }
}