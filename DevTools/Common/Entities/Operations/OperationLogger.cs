using System.Threading.Tasks;
using NLog;
using VXDesign.Store.DevTools.Common.Storage.LogStores;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public interface IOperationLogger
    {
        Task Trace(string message, dynamic value = null);
        Task Debug(string message, dynamic value = null);
        Task Info(string message, dynamic value = null);
        Task Warn(string message, dynamic value = null);
        Task Error(string message, dynamic value = null);
        Task Fatal(string message, dynamic value = null);

        Task<long> CountOfAllCollections();
    }

    public class OperationLogger<T> : IOperationLogger
    {
        private readonly long operationId;
        private readonly ILogger logger;
        private readonly ILoggerStore loggerStore;

        public OperationLogger(ILoggerStore loggerStore, long operationId)
        {
            this.loggerStore = loggerStore;
            this.operationId = operationId;
            logger = LogManager.GetLogger(typeof(T).FullName);
        }

        public async Task Trace(string message, dynamic value = null)
        {
            logger.Trace(message);
            await loggerStore.Trace<T>(operationId, message, value);
        }

        public async Task Debug(string message, dynamic value = null)
        {
            logger.Debug(message);
            await loggerStore.Debug<T>(operationId, message, value);
        }

        public async Task Info(string message, dynamic value = null)
        {
            logger.Info(message);
            await loggerStore.Info<T>(operationId, message, value);
        }

        public async Task Warn(string message, dynamic value = null)
        {
            logger.Warn(message);
            await loggerStore.Warn<T>(operationId, message, value);
        }

        public async Task Error(string message, dynamic value = null)
        {
            logger.Error(message);
            await loggerStore.Error<T>(operationId, message, value);
        }

        public async Task Fatal(string message, dynamic value = null)
        {
            logger.Fatal(message);
            await loggerStore.Fatal<T>(operationId, message, value);
        }

        public async Task<long> CountOfAllCollections() => await loggerStore.CountOfAllCollections();
    }
}