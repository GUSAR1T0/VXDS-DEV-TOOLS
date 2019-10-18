using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Storage;

namespace VXDesign.Store.DevTools.Common.Storage.LogStores
{
    public interface ILoggerStore
    {
        Task Trace<T>(string operationId, string message, object value);
        Task Debug<T>(string operationId, string message, object value);
        Task Info<T>(string operationId, string message, object value);
        Task Warn<T>(string operationId, string message, object value);
        Task Error<T>(string operationId, string message, object value);
        Task Fatal<T>(string operationId, string message, object value);
    }

    public class LoggerStore : BaseLogStore<LoggerEntity>, ILoggerStore
    {
        public LoggerStore(string logStoreConnectionString) : base(logStoreConnectionString, "Logs")
        {
        }

        public async Task Trace<T>(string operationId, string message, object value)
        {
            await Log<T>("TRACE", operationId, message, value);
        }

        public async Task Debug<T>(string operationId, string message, object value)
        {
            await Log<T>("DEBUG", operationId, message, value);
        }

        public async Task Info<T>(string operationId, string message, object value)
        {
            await Log<T>("INFO", operationId, message, value);
        }

        public async Task Warn<T>(string operationId, string message, object value)
        {
            await Log<T>("WARN", operationId, message, value);
        }

        public async Task Error<T>(string operationId, string message, object value)
        {
            await Log<T>("ERROR", operationId, message, value);
        }

        public async Task Fatal<T>(string operationId, string message, object value)
        {
            await Log<T>("FATAL", operationId, message, value);
        }

        private async Task Log<T>(string level, string operationId, string message, object value)
        {
            await Collection.InsertOneAsync(new LoggerEntity
            {
                Level = level,
                Logger = typeof(T).FullName,
                OperationId = operationId,
                Message = message,
                Value = value
            });
        }
    }
}