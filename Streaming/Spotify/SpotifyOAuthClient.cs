using System.Data;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PlaylistSync.Auth.Implementations;
using PlaylistSync.Auth.Models;

namespace PlaylistSync.Streaming.Spotify;

internal class SpotifyOAuthClient(ILogger<OAuthClientBase> logger, HttpClient httpClient, IMemoryCache cache) : OAuthClientBase
{
    private const string CacheKey = "Spotify:ClientCredentialsToken";

    public override async Task<ClientCredentialsTokenResponse> RequestTokenAsync(ClientCredentialsRequest clientCredentialsRequest, CancellationToken cancellationToken = default)
    {
        cache.TryGetValue<ClientCredentialsTokenResponse>(CacheKey, out var cachedToken);

        if (cachedToken is not null && cachedToken.CreatedAt.AddSeconds(cachedToken.ExpiresIn) > DateTimeOffset.UtcNow.AddMinutes(1))
        {
            logger.LogDebug("Using cached client credentials token");
            return cachedToken;
        }

        logger.LogInformation("Requesting new client credentials token");

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        request.Headers.Authorization = new("Basic", EncodeClientCredentials(clientCredentialsRequest.ClientId, clientCredentialsRequest.ClientSecret));

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" }
        });

        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadFromJsonAsync<ClientCredentialsTokenResponse>(cancellationToken) ?? throw new NoNullAllowedException("Failed to deserialize token response.");

        cache.Set(CacheKey, token, TimeSpan.FromSeconds(token.ExpiresIn) - TimeSpan.FromSeconds(30));

        return token;
    }

    public override async Task<AuthorizationCodeTokenResponse> RequestTokenAsync(AuthorizationCodeRequest authorizationCodeRequest, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        request.Headers.Authorization = new("Basic", EncodeClientCredentials(authorizationCodeRequest.ClientId, authorizationCodeRequest.ClientSecret));

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "code", authorizationCodeRequest.Code },
            { "redirect_uri", authorizationCodeRequest.RedirectUri.ToString() }
        });

        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AuthorizationCodeTokenResponse>(cancellationToken) ?? throw new NoNullAllowedException("Failed to deserialize token response.");
    }
}
