namespace PlaylistSync.Auth;

public interface IToken
{
    public string AccessToken { get; set; }

    public string TokenType { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsExpired { get; }
}
