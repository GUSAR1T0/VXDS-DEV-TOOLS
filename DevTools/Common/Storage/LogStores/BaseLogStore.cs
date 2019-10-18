using MongoDB.Driver;

namespace VXDesign.Store.DevTools.Common.Storage.LogStores
{
    public abstract class BaseLogStore<TEntity>
    {
        protected IMongoCollection<TEntity> Collection { get; }

        protected BaseLogStore(string logStoreConnectionString, string collection)
        {
            var urlToMongoDb = new MongoUrl(logStoreConnectionString);
            var databaseName = urlToMongoDb.DatabaseName;
            var connectionStringWithoutDatabaseName = logStoreConnectionString.Remove(logStoreConnectionString.Length - urlToMongoDb.DatabaseName.Length - 1);
            Collection = new MongoClient(connectionStringWithoutDatabaseName).GetDatabase(databaseName).GetCollection<TEntity>(collection);
        }
    }
}