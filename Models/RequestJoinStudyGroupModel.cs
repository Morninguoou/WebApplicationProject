using System.Text.Json.Serialization;
using marian_onsite.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;



namespace marian_onsite.Models
{
    public class RequestJoinStudyGroup
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("studyGroupId")]
        [BsonElement("study_group_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ?StudyGroupId { get; set; } = string.Empty;

        [JsonPropertyName("userId")]
        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ?UserId { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public RequestJoinStudyGroupStatus? Status { get; set; } = RequestJoinStudyGroupStatus.Pending;


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