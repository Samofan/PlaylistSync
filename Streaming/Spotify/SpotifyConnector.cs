using Microsoft.Extensions.Logging;
using PlaylistSync.Auth.Implementations;
using PlaylistSync.Auth.Models;
using PlaylistSync.Models;

namespace PlaylistSync.Streaming.Spotify;

internal sealed class SpotifyConnector(ILogger<SpotifyConnector> logger, SpotifyOAuthClient authClient, HttpClient httpClient) : IStreamingServiceConnector
{
    public async Task<Album?> SearchAlbumAsync(Album album, CancellationToken cancellationToken)
    {
        logger.LogDebug("Searching for album '{AlbumName}'", album.Title);

        var token = await authClient.RequestTokenAsync(new ClientCredentialsRequest("ClientId", "ClientSecret"), cancellationToken);

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/search?q=album:{Uri.EscapeDataString(album.Title)}%20artist:{Uri.EscapeDataString(album.Artists.Select(a => a.Name).FirstOrDefault() ?? "")}&type=album&limit=1");

        request.Headers.Authorization = new("Bearer", token.AccessToken);

        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        // TODO: Just to illustrate deserialization, not a complete implementation
        //var json = await response.Content.ReadFromJsonAsync<SpotifySearchResponse>(cancellationToken) ?? throw new Exception("Failed to deserialize search response.");

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        logger.LogDebug("Search response: {Response}", json);

        return new Album(
            album.Title,
            album.Year,
            album.Artists
        );
    }
}
