using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VXDesign.Store.DevTools.Core.Entities.Storage
{
    public class LoggerEntity : IDataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Level { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public string Logger { get; set; }

        public long OperationId { get; set; }

        public string Message { get; set; }

        public dynamic Value { get; set; }
    }
}