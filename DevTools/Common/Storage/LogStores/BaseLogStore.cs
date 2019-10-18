using MongoDB.Driver;

namespace VXDesign.Store.DevTools.Common.Storage.LogStores
{
    public abstract class BaseLogStore<TEntity>
    {
        protected IMongoCollection<TEntity> Collection { get; }

        protected BaseLogStore(string logStoreConnectionString, string schema, string collection)
        {
            Collection = new MongoClient(logStoreConnectionString).GetDatabase(schema).GetCollection<TEntity>(collection);
        }
    }
}