using System.Text.Json.Serialization;

namespace PlaylistSync.Auth.Models;

public class ClientCredentialsTokenResponse : IToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = default!;
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsExpired => CreatedAt.AddSeconds(ExpiresIn) <= DateTime.UtcNow;
}
