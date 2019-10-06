using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.Containers.Properties;

namespace VXDesign.Store.DevTools.Common.DataStorage.Stores
{
    public abstract class BaseDataStore
    {
        protected IMongoDatabase Client { get; }

        protected BaseDataStore(DatabaseConnectionProperties properties)
        {
            Client = new MongoClient(properties.ConnectionString).GetDatabase(properties.Database);
        }
    }
}