using System.Text.Json.Serialization;

namespace PlaylistSync.Streaming.Spotify.Models;

internal class AlbumSearchResponseImage
{
    [JsonPropertyName("height")]
    public required int Height { get; set; }
    [JsonPropertyName("width")]
    public required int Width { get; set; }
    [JsonPropertyName("url")]
    public required string Url { get; set; }
}
