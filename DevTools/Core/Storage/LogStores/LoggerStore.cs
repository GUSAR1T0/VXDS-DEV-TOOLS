using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Core.Entities.Storage;

namespace VXDesign.Store.DevTools.Core.Storage.LogStores
{
    public interface ILoggerStore
    {
        Task Trace<T>(long operationId, string message, dynamic value = null);
        Task Debug<T>(long operationId, string message, dynamic value = null);
        Task Info<T>(long operationId, string message, dynamic value = null);
        Task Warn<T>(long operationId, string message, dynamic value = null);
        Task Error<T>(long operationId, string message, dynamic value = null);
        Task Fatal<T>(long operationId, string message, dynamic value = null);

        Task<long> CountOfAllCollections();
        Task DropAllLogCollections();
    }

    public class LoggerStore : BaseLogStore<LoggerEntity>, ILoggerStore
    {
        public LoggerStore(string logStoreConnectionString, string scope) : base(logStoreConnectionString, "logs", scope)
        {
        }

        public async Task Trace<T>(long operationId, string message, dynamic value = null)
        {
            await Log<T>("TRACE", operationId, message, value);
        }

        public async Task Debug<T>(long operationId, string message, dynamic value = null)
        {
            await Log<T>("DEBUG", operationId, message, value);
        }

        public async Task Info<T>(long operationId, string message, dynamic value = null)
        {
            await Log<T>("INFO", operationId, message, value);
        }

        public async Task Warn<T>(long operationId, string message, dynamic value = null)
        {
            await Log<T>("WARN", operationId, message, value);
        }

        public async Task Error<T>(long operationId, string message, dynamic value = null)
        {
            await Log<T>("ERROR", operationId, message, value);
        }

        public async Task Fatal<T>(long operationId, string message, dynamic value = null)
        {
            await Log<T>("FATAL", operationId, message, value);
        }

        private async Task Log<T>(string level, long operationId, string message, dynamic value)
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

        public async Task<long> CountOfAllCollections()
        {
            var count = 0L;
            var collectionNamesCursor = await Database.ListCollectionNamesAsync();
            var collectionNames = await collectionNamesCursor.ToListAsync();
            foreach (var collectionName in collectionNames)
            {
                var collection = Database.GetCollection<dynamic>(collectionName);
                count += await collection.CountDocumentsAsync(FilterDefinition<dynamic>.Empty);
            }

            return count;
        }

        public async Task DropAllLogCollections()
        {
            var collectionNamesCursor = await Database.ListCollectionNamesAsync();
            var collectionNames = await collectionNamesCursor.ToListAsync();
            foreach (var collectionName in collectionNames)
            {
                await Database.DropCollectionAsync(collectionName);
            }
        }
    }
}