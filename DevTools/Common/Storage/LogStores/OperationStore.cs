using System.Threading.Tasks;
using MongoDB.Driver;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Entities.Storage;

namespace VXDesign.Store.DevTools.Common.Storage.LogStores
{
    public interface IOperationStore
    {
        Task<string> Start(int userId, OperationContext context);
        Task Stop(string operationId, bool isSuccess);
    }

    public class OperationStore : BaseLogStore<OperationEntity>, IOperationStore
    {
        public OperationStore(string logStoreConnectionString) : base(logStoreConnectionString, "Operations")
        {
        }

        public async Task<string> Start(int userId, OperationContext context)
        {
            var record = new OperationEntity
            {
                UserId = userId,
                OperationContext = context
            };
            await Collection.InsertOneAsync(record);
            return record.Id;
        }

        public async Task Stop(string operationId, bool isSuccess)
        {
            await Collection.UpdateOneAsync(entity => entity.Id == operationId && entity.IsSuccess == null,
                Builders<OperationEntity>.Update.Set(entity => entity.IsSuccess, isSuccess));
        }
    }
}