using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VXDesign.Store.DevTools.Common.Entities.Storage
{
    public class LoggerEntity : IDataEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Level { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public string Logger { get; set; }

        public int OperationId { get; set; }

        public string Message { get; set; }

        public dynamic Value { get; set; }
    }
}