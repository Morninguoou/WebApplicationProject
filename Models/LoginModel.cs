using System.Text.Json.Serialization;

namespace marian_onsite.Models;
public class Login
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;
}