using System.Text.Json.Serialization;

namespace PlaylistSync.Streaming.Spotify.Models;

internal class AlbumSearchResponseArtist
{
    [JsonPropertyName("external_urls")]
    public required Dictionary<string, string> ExternalUrls { get; set; }
    [JsonPropertyName("href")]
    public required string Href { get; set; }
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("type")]
    public required string Type { get; set; }
    [JsonPropertyName("uri")]
    public required string Uri { get; set; }
}
