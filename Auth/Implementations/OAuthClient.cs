using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace PlaylistSync.Auth.Implementations;

// https://github.com/JohnnyCrazy/SpotifyAPI-NET/blob/master/SpotifyAPI.Web/Clients/OAuthClient.cs#L268

public class OAuthClient(ILogger<OAuthClient> logger) : IOAuthClient
{
    public Task<ClientCredentialsTokenResponse> RequestTokenAsync(ClientCredentialsRequest clientCredentialsRequest, CancellationToken cancellationToken = default)
    {
        var form = new List<KeyValuePair<string?, string?>>
        {
            new("grant_type", "client_credentials")
        };

        return SendOAuthRequestAsync<ClientCredentialsTokenResponse>(form, clientCredentialsRequest.ClientId, clientCredentialsRequest.ClientSecret, cancellationToken);
    }

    public Task<AuthorizationCodeTokenResponse> RequestTokenAsync(AuthorizationCodeRequest authorizationCodeRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private Task<T> SendOAuthRequestAsync<T>(List<KeyValuePair<string?, string?>> form, string? clientId, string? clientSecret, CancellationToken cancellationToken = default) where T : IToken
    {
        var headers = BuildAuthHeader(clientId, clientSecret);

        throw new NotImplementedException();
        //return apiConnector.Post<T>(SpotifyUrls.OAuthToken, null, new FormUrlEncodedContent(form), headers, cancellationToken);
    }

    private static Dictionary<string, string> BuildAuthHeader(string? clientId, string? clientSecret)
    {
        if (clientId == null || clientSecret == null)
        {
            return [];
        }

        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        return new Dictionary<string, string>
        {
            { "Authorization", $"Basic {base64}"}
        };
    }
}
