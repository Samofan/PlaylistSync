namespace PlaylistSync.Auth;

public record AuthorizationCodeRequest(string ClientId, string ClientSecret, string Code, Uri RedirectUri);
