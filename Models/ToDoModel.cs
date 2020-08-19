using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDoApi.Models
{
    public class ToDoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Activity { get; set; }
        public string Description { get; set; }
        public DateTime DeadLine  { get; set; }
        public bool Completed { get; set; }
    }
}