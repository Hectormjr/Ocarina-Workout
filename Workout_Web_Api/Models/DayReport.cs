using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Workout_Web_Api.Models
{
    public class DayReport 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Calories")]
        public List<int> Calories { get; set; }

        [BsonElement("Intensity")]
        public List<int> Intensity { get; set; }

        [BsonElement("Exercise")]
        public List<string> Exercise{ get; set; }

        [BsonElement("CaloriesThresholdReached")]
        public bool CaloriesThresholdReached { get; set; }

        [BsonElement("TotalCalories")]
        public int TotalCalories { get; set; }

        [BsonElement("Variation")]
        public int Variation { get; set; }
    }
}
