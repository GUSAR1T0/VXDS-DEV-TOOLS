using MongoDB.Driver;

namespace VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores
{
    public abstract class BaseLogStore<TEntity>
    {
        protected IMongoDatabase Database { get; }
        protected IMongoCollection<TEntity> Collection { get; }

        protected BaseLogStore(string logStoreConnectionString, string schema, string collection)
        {
            Database = new MongoClient(logStoreConnectionString).GetDatabase(schema);
            Collection = Database.GetCollection<TEntity>(collection);
        }
    }
}