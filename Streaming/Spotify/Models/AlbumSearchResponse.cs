using System.Text.Json.Serialization;

namespace PlaylistSync.Streaming.Spotify.Models;

internal class AlbumSearchResponse
{
    [JsonPropertyName("albums")]
    public required AlbumSearchResponseAlbums Albums { get; set; }
}
