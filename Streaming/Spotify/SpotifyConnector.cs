using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlaylistSync.Auth;
using PlaylistSync.Auth.Models;
using PlaylistSync.Models;
using PlaylistSync.Models.Settings;
using PlaylistSync.Streaming.Spotify.Models;

namespace PlaylistSync.Streaming.Spotify;

internal sealed class SpotifyConnector(ILogger<SpotifyConnector> logger, IOptions<ApplicationSettings> options, IOAuthClient authClient, HttpClient httpClient) : IStreamingServiceConnector
{
    public async Task<Album?> SearchAlbumAsync(AlbumSearchRequest albumSearchRequest, CancellationToken cancellationToken)
    {
        logger.LogDebug("Searching for album '{AlbumName}'", albumSearchRequest.Title);

        var clientId = options.Value.StreamingServiceSettings.ClientId;
        var clientSecret = options.Value.StreamingServiceSettings.ClientSecret;

        var token = await authClient.RequestTokenAsync(new ClientCredentialsRequest(clientId, clientSecret), cancellationToken);

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/search?q=album:{Uri.EscapeDataString(albumSearchRequest.Title)}%20artist:{Uri.EscapeDataString(albumSearchRequest.Artist)}&type=album&limit=1");

        request.Headers.Authorization = new("Bearer", token.AccessToken);

        var response = await httpClient.SendAsync(request, cancellationToken);

        try
        {
            response.EnsureSuccessStatusCode();

            var albumSearchResponse = await response.Content.ReadFromJsonAsync<AlbumSearchResponse>(cancellationToken) ?? throw new Exception("Failed to deserialize search response.");

            // TODO: Validate search result
            var firstItem = albumSearchResponse.Albums.Items.FirstOrDefault();
            var title = firstItem?.Name ?? string.Empty;
            var artists = firstItem?.Artists.Select(a => new Artist(a.Name)).ToList() ?? new List<Artist>();
            var year = 14; // You may want to parse the year from firstItem?.ReleaseDate

            return new Album(title, year, artists);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request failed while searching for album '{AlbumName}'. Response {Response}", albumSearchRequest.Title, await response.Content.ReadAsStringAsync(cancellationToken));
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while searching for album '{AlbumName}'. Response {Response}", albumSearchRequest.Title, await response.Content.ReadAsStringAsync(cancellationToken));
            throw;
        }
    }
}
