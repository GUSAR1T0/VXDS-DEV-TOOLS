using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VXDesign.Store.DevTools.Common.DataStorage.Entities
{
    public class UserRoleEntity : IDataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}