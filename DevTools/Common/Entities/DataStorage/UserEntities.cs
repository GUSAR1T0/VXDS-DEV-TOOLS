using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VXDesign.Store.DevTools.Common.Entities.DataStorage
{
    public abstract class GeneralUserEntity : IDataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserAuthorizationEntity : GeneralUserEntity
    {
        public string RefreshToken { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class UserRegistrationEntity : GeneralUserEntity
    {
        public string Password { get; set; }
    }
}