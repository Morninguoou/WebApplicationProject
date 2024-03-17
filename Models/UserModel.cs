using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace marian_onsite.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("password")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("avatarUrl")]
        [BsonElement("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        [BsonElement("phone")]
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [BsonElement("bio")]
        [JsonPropertyName("bio")]
        public string? Bio { get; set; } = string.Empty;

        [BsonElement("followers")]
        [JsonPropertyName("followers")]
        public List<string>? Followers { get; set; } = new List<string>();

        [BsonElement("following")]
        [JsonPropertyName("following")]
        public List<string>? Following { get; set; } = new List<string>();

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