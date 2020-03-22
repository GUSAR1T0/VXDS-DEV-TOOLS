using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Entities;

namespace VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores
{
    public interface ILoggerStore
    {
        Task Trace<T>(long operationId, string message, dynamic value = null);
        Task Debug<T>(long operationId, string message, dynamic value = null);
        Task Info<T>(long operationId, string message, dynamic value = null);
        Task Warn<T>(long operationId, string message, dynamic value = null);
        Task Error<T>(long operationId, string message, dynamic value = null);
        Task Fatal<T>(long operationId, string message, dynamic value = null);

        Task<(IEnumerable<LogsDataByDateEntity> logs, long total)> GetLogsData(DateTime sevenDaysAgo, DateTime today);
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

        public async Task<(IEnumerable<LogsDataByDateEntity> logs, long total)> GetLogsData(DateTime sevenDaysAgo, DateTime today)
        {
            var total = 0L;

            var dates = new List<DateTime>();
            var filterBuilder = Builders<LogEntity>.Filter;
            var filters = new Dictionary<DateTime, FilterDefinition<LogEntity>>();
            for (var date = sevenDaysAgo; date <= today; date = date.AddDays(1))
            {
                dates.Add(date);
                filters.Add(date, filterBuilder.Gte(x => x.DateTime, date) & filterBuilder.Lt(x => x.DateTime, date.AddDays(1)));
            }

            var dictionary = dates.ToDictionary(date => date, date => 0L);

            foreach (var collectionName in await GetCollectionNames())
            {
                var collection = Database.GetCollection<LogEntity>(collectionName);
                total += await collection.CountDocumentsAsync(FilterDefinition<LogEntity>.Empty);
                foreach (var (date, filter) in filters)
                {
                    dictionary[date] += await collection.CountDocumentsAsync(filter);
                }
            }

            return (dictionary.Select(item => new LogsDataByDateEntity
            {
                Date = item.Key,
                Count = item.Value
            }), total);
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