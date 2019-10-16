using VXDesign.Store.DevTools.Common.Containers.Properties;

namespace VXDesign.Store.DevTools.Common.DataStorage.Stores
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