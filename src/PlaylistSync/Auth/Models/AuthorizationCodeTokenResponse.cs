namespace PlaylistSync.Auth.Models;

public class AuthorizationCodeTokenResponse : IRefreshableToken
  {
    public string AccessToken { get; set; } = default!;
    public string TokenType { get; set; } = default!;
    public int ExpiresIn { get; set; }
    public string Scope { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsExpired { get => CreatedAt.AddSeconds(ExpiresIn) <= DateTime.UtcNow; }
  }