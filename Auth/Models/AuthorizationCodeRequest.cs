namespace PlaylistSync.Auth.Models;

public record AuthorizationCodeRequest(string ClientId, string ClientSecret, string Code, Uri RedirectUri);
