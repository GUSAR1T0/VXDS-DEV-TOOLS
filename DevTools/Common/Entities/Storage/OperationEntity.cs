using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Entities.Storage
{
    public class OperationEntity : IDataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int UserId { get; set; }

        public OperationContext OperationContext { get; set; }

        public bool? IsSuccess { get; set; }
    }
}