using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Workout_Web_Api.Models
{
    public class WeekReport
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Week")]
        public double Week{ get; set; }

        [BsonElement("Month")]
        public int Month{ get; set; }

        [BsonElement("Sunday")]
        public DayReport Sunday { get; set; }

        [BsonElement("Monday")]
        public DayReport Monday { get; set; }

        [BsonElement("Tuesday")]
        public DayReport Tuesday { get; set; }

        [BsonElement("Wednesday")]
        public DayReport Wednesday { get; set; }

        [BsonElement("Thursday")]
        public DayReport Thursday { get; set; }

        [BsonElement("Friday")]
        public DayReport Friday { get; set; }

        [BsonElement("Saturday")]
        public DayReport Saturday { get; set; }

    }
}
