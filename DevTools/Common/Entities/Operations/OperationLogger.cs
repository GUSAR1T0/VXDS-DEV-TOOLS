using System.Threading.Tasks;
using NLog;
using VXDesign.Store.DevTools.Common.Storage.LogStores;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public interface IOperationLogger
    {
        Task Trace(string message, object value);
        Task Debug(string message, object value);
        Task Info(string message, object value);
        Task Warn(string message, object value);
        Task Error(string message, object value);
        Task Fatal(string message, object value);
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

        public async Task Trace(string message, object value)
        {
            logger.Trace(message);
            await loggerStore.Trace<T>(operationId, message, value);
        }

        public async Task Debug(string message, object value)
        {
            logger.Debug(message);
            await loggerStore.Debug<T>(operationId, message, value);
        }

        public async Task Info(string message, object value)
        {
            logger.Info(message);
            await loggerStore.Info<T>(operationId, message, value);
        }

        public async Task Warn(string message, object value)
        {
            logger.Warn(message);
            await loggerStore.Warn<T>(operationId, message, value);
        }

        public async Task Error(string message, object value)
        {
            logger.Error(message);
            await loggerStore.Error<T>(operationId, message, value);
        }

        public async Task Fatal(string message, object value)
        {
            logger.Fatal(message);
            await loggerStore.Fatal<T>(operationId, message, value);
        }
    }
}