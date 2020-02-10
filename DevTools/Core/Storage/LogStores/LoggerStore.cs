using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Core.Entities.Storage.Log;

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

        Task<long> GetCount();
        Task DropAllLogCollections();

        Task<IEnumerable<LogEntity>> GetByOperations(IEnumerable<long> operationIds);
    }

    public class LoggerStore : BaseLogStore<LogEntity>, ILoggerStore
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
            await Collection.InsertOneAsync(new LogEntity
            {
                Level = level,
                Logger = typeof(T).FullName,
                OperationId = operationId,
                Message = message,
                Value = value
            });
        }

        private async Task<IEnumerable<string>> GetCollectionNames()
        {
            var collectionNamesCursor = await Database.ListCollectionNamesAsync();
            return await collectionNamesCursor.ToListAsync();
        }

        public async Task<long> GetCount()
        {
            var count = 0L;
            foreach (var collectionName in await GetCollectionNames())
            {
                var collection = Database.GetCollection<dynamic>(collectionName);
                count += await collection.CountDocumentsAsync(FilterDefinition<dynamic>.Empty);
            }

            return count;
        }

        public async Task DropAllLogCollections()
        {
            foreach (var collectionName in await GetCollectionNames())
            {
                await Database.DropCollectionAsync(collectionName);
            }
        }

        public async Task<IEnumerable<LogEntity>> GetByOperations(IEnumerable<long> operationIds)
        {
            var logs = new List<LogEntity>();
            foreach (var collectionName in await GetCollectionNames())
            {
                var collection = Database.GetCollection<LogEntity>(collectionName);
                logs.AddRange(await collection.Find(document => operationIds.Contains(document.OperationId)).ToListAsync());
            }

            return logs;
        }
    }
}