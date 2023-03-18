using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Workout_Web_Api.Models
{
    public class Workout
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Exercise")]
        public string Exercise { get; set; }

        [BsonElement("Calories")]
        public int Calories { get; set; }

        [BsonElement("Intensity")]
        public int Intensity { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }
    }
}
