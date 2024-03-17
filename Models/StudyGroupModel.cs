using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using marian_onsite.Enums;

namespace marian_onsite.Models
{
    public class StudyGroup
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        [BsonElement("title")]
        [BsonRepresentation(BsonType.String)]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        [BsonElement("description")]
        [BsonRepresentation(BsonType.String)]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("subject")]
        [BsonElement("subject")]
        [BsonRepresentation(BsonType.String)]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        [BsonElement("content")]
        [BsonRepresentation(BsonType.String)]
        public string Content { get; set; } = string.Empty;


        [JsonPropertyName("status")]
        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public StudyGroupStatus? Status { get; set; } = StudyGroupStatus.Pending;

        [JsonPropertyName("location")]
        [BsonElement("location")]
        public Location Location { get; set; } = new Location();

        [JsonPropertyName("maxMember")]
        [BsonElement("max_member")]
        [BsonRepresentation(BsonType.Int32)]
        public int MaxMember { get; set; } = 0;

        [JsonPropertyName("members")]
        [BsonElement("members")]
        public List<string>? Members { get; set; } = new List<string>();


        [JsonPropertyName("creator")]
        [BsonElement("creator")]
        public string Creator { get; set; } = string.Empty;

        [JsonPropertyName("expiration")]
        [BsonElement("expiration")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Expiration { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("appointment")]
        [BsonElement("appointment")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Appointment { get; set; } = DateTime.UtcNow;


        [JsonPropertyName("createdAt")]
        [BsonElement("created_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("updatedAt")]
        [BsonElement("updated_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
