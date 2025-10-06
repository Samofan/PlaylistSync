using PlaylistSync.Auth.Models;

namespace PlaylistSync.Auth.Implementations;

public abstract class OAuthClientBase : IOAuthClient
{
    public abstract Task<ClientCredentialsTokenResponse> RequestTokenAsync(ClientCredentialsRequest clientCredentialsRequest, CancellationToken cancellationToken = default);
    public abstract Task<AuthorizationCodeTokenResponse> RequestTokenAsync(AuthorizationCodeRequest authorizationCodeRequest, CancellationToken cancellationToken = default);

    protected static string EncodeClientCredentials(string clientId, string clientSecret)
    {
        var raw = $"{clientId}:{clientSecret}";
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(raw));
    }
}
