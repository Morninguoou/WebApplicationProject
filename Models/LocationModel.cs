using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace marian_onsite.Models;
public class Location
{

    [JsonPropertyName("latitude")]
    [BsonElement("latitude")]
    public string Latitude { get; set; } = null!;

    [JsonPropertyName("longitude")]
    [BsonElement("longitude")]
    public string Longitude { get; set; } = null!;

    [JsonPropertyName("address")]
    [BsonElement("address")]
    public string? Address { get; set; } = null!;
}