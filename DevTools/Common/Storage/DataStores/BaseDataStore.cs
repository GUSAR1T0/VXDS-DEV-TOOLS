using VXDesign.Store.DevTools.Common.Entities.Properties;

namespace VXDesign.Store.DevTools.Common.Storage.DataStores
{
    public abstract class BaseDataStore
    {
        protected DatabaseConnectionProperties Properties { get; }

        protected BaseDataStore(DatabaseConnectionProperties properties)
        {
            Properties = properties;
        }
    }
}