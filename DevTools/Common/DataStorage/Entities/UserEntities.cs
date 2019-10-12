using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VXDesign.Store.DevTools.Common.DataStorage.Entities
{
    public abstract class GeneralUserEntity : IDataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Color { get; set; }
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

    [BsonIgnoreExtraElements]
    public class FullUserDataEntity : GeneralUserEntity
    {
    }
}