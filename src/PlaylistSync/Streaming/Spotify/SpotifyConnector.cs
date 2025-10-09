using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlaylistSync.Models;
using PlaylistSync.Models.Settings;
using PlaylistSync.Streaming.Spotify.Models;

namespace PlaylistSync.Streaming.Spotify;

internal sealed class SpotifyConnector(ILogger<SpotifyConnector> logger, SpotifyApiClient spotifyApiClient) : IStreamingServiceConnector
{
    public async Task<Album?> SearchAlbumAsync(AlbumSearchRequest albumSearchRequest, CancellationToken cancellationToken)
    {
        var query = $"search?q=album:{Uri.EscapeDataString(albumSearchRequest.Title)}%20artist:{Uri.EscapeDataString(albumSearchRequest.Artist)}&type=album&limit=1";

        AlbumSearchResponse? albumSearchResponse = null;

        try
        {
            albumSearchResponse = await spotifyApiClient.GetAsync<AlbumSearchResponse>(query, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while searching for album '{AlbumName}'.", albumSearchRequest.Title);
            return default;
        }

        if (albumSearchResponse is null || !albumSearchResponse.Albums.Items.Any())
        {
            logger.LogWarning("No albums found when searching for album '{AlbumName}'.", albumSearchRequest.Title);
            return default;
        }

        var foundAlbum = albumSearchResponse.Albums.Items.First();
        var releaseYear = 0;
        
        try
        {
            releaseYear = ExtractYear(foundAlbum.ReleaseDate);
        }
        catch (FormatException ex)
        {
            logger.LogWarning(ex, "Failed to parse release date '{ReleaseDate}' for album '{AlbumName}'. Setting year to 0.", foundAlbum.ReleaseDate, albumSearchRequest.Title);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Release date is empty for album '{AlbumName}'. Setting year to 0.", albumSearchRequest.Title);
        }

        return new SpotifyAlbum(
            foundAlbum.Id,
            foundAlbum.Name,
            releaseYear,
            foundAlbum.Artists.Select(a => new SpotifyArtist(a.Name)).ToList<Artist>()
        );
    }

    private static int ExtractYear(string releaseDate)
    {
        if (string.IsNullOrWhiteSpace(releaseDate))
            throw new ArgumentException("Date string must not be empty.", nameof(releaseDate));

        var parts = releaseDate.Split('-');

        if (parts.Length == 0 || !int.TryParse(parts[0], out int year))
            throw new FormatException($"Invalid date format: '{releaseDate}'");

        return year;
    }
}
