using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.Containers.Properties;

namespace VXDesign.Store.DevTools.Common.DataStorage.Stores
{
    public abstract class BaseDataStore
    {
        protected IMongoDatabase Client { get; }

        protected BaseDataStore(IMongoDatabase client)
        {
            Client = client;
        }

        public static IMongoDatabase Initialize(DatabaseConnectionProperties properties)
        {
            return new MongoClient(properties.ConnectionString).GetDatabase(properties.Database);
        }
    }
}