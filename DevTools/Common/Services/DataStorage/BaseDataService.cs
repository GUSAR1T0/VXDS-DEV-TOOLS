using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.Containers.Properties;

namespace VXDesign.Store.DevTools.Common.Services.DataStorage
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