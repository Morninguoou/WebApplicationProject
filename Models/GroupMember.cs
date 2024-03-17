//  {
//                 Id = _member.Id,
//                 AvatarUrl = _member.AvatarUrl,
//                 Username = _member.Username,
//                 IsFollowing = _member.Followers.Contains(user.Id)
            
using System.Text.Json.Serialization;

namespace marian_onsite.Models;

public class GroupMember
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("avatarUrl")]
    public string AvatarUrl { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("isFollowing")]
    public bool IsFollowing { get; set; }
}