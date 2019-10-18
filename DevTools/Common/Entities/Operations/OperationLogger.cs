using System.Threading.Tasks;
using NLog;
using VXDesign.Store.DevTools.Common.Storage.LogStores;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public interface IOperationLogger
    {
        Task Trace(string message, object value = null);
        Task Debug(string message, object value = null);
        Task Info(string message, object value = null);
        Task Warn(string message, object value = null);
        Task Error(string message, object value = null);
        Task Fatal(string message, object value = null);
    }

    public class OperationLogger<T> : IOperationLogger
    {
        private readonly string operationId;
        private readonly ILogger logger;
        private readonly ILoggerStore loggerStore;

        public OperationLogger(ILoggerStore loggerStore, string operationId)
        {
            this.loggerStore = loggerStore;
            this.operationId = operationId;
            logger = LogManager.GetLogger(typeof(T).FullName);
        }

        public async Task Trace(string message, object value = null)
        {
            logger.Trace(message);
            await loggerStore.Trace<T>(operationId, message, value);
        }

        public async Task Debug(string message, object value = null)
        {
            logger.Debug(message);
            await loggerStore.Debug<T>(operationId, message, value);
        }

        public async Task Info(string message, object value = null)
        {
            logger.Info(message);
            await loggerStore.Info<T>(operationId, message, value);
        }

        public async Task Warn(string message, object value = null)
        {
            logger.Warn(message);
            await loggerStore.Warn<T>(operationId, message, value);
        }

        public async Task Error(string message, object value = null)
        {
            logger.Error(message);
            await loggerStore.Error<T>(operationId, message, value);
        }

        public async Task Fatal(string message, object value = null)
        {
            logger.Fatal(message);
            await loggerStore.Fatal<T>(operationId, message, value);
        }
    }
}