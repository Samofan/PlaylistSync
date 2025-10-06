using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlaylistSync.Auth;
using PlaylistSync.Auth.Models;
using PlaylistSync.Models;
using PlaylistSync.Models.Settings;
using PlaylistSync.Streaming.Spotify.Models;

namespace PlaylistSync.Streaming.Spotify;

internal sealed class SpotifyConnector(ILogger<SpotifyConnector> logger, IOptions<ApplicationSettings> options, SpotifyApiClient spotifyApiClient) : IStreamingServiceConnector
{
    public async Task<Album?> SearchAlbumAsync(AlbumSearchRequest albumSearchRequest, CancellationToken cancellationToken)
    {
        logger.LogDebug("Searching for album '{AlbumName}'", albumSearchRequest.Title);

        var query = $"search?q=album:{Uri.EscapeDataString(albumSearchRequest.Title)}%20artist:{Uri.EscapeDataString(albumSearchRequest.Artist)}&type=album&limit=1";

        
        try
        {
            var albumSearchResponse = await spotifyApiClient.GetAsync<AlbumSearchResponse>(query, cancellationToken);

            // TODO: Validate search result
            var firstItem = albumSearchResponse.Albums.Items.FirstOrDefault();
            var title = firstItem?.Name ?? string.Empty;
            var artists = firstItem?.Artists.Select(a => new Artist(a.Name)).ToList() ?? new List<Artist>();
            var year = 14; // You may want to parse the year from firstItem?.ReleaseDate

            return new Album(title, year, artists);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while searching for album '{AlbumName}'.", albumSearchRequest.Title);
            throw;
        }
    }
}
