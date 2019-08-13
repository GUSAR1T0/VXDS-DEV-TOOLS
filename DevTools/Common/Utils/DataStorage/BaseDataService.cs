using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Utils.DataStorage
{
    public abstract class BaseDataService
    {
        protected IMongoDatabase Client { get; }

        protected BaseDataService(DatabaseConnectionProperties properties)
        {
            Client = new MongoClient(properties.ConnectionString).GetDatabase(properties.Database);
        }
    }
}