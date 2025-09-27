using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LocaRider.Domain.Events
{
    public class MotorcycleNotification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string MotorcycleId { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
