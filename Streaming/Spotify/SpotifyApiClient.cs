using System;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PlaylistSync.Common;

namespace PlaylistSync.Streaming.Spotify;

internal sealed class SpotifyApiClient(ILogger<SpotifyApiClient> logger, HttpClient httpClient) : IApiClient
{
    public async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync(relativeUrl, cancellationToken);

        try
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Spotify API request failed for {Url}", relativeUrl);
            throw;
        }
    }
}
