using System.Text.Json.Serialization;

namespace PlaylistSync.Streaming.Spotify.Models;

internal class AlbumSearchResponseItem
{
    [JsonPropertyName("album_type")]
    public required string AlbumType { get; set; }
    [JsonPropertyName("total_tracks")]
    public required int TotalTracks { get; set; }
    [JsonPropertyName("available_markets")]
    public required IEnumerable<string> AvailableMarkets { get; set; }
    [JsonPropertyName("external_urls")]
    public required Dictionary<string, string> ExternalUrls { get; set; }
    [JsonPropertyName("href")]
    public required string Href { get; set; }
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    [JsonPropertyName("images")]
    public required IEnumerable<AlbumSearchResponseImage> Images { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("release_date")]
    public required string ReleaseDate { get; set; }
    [JsonPropertyName("release_date_precision")]
    public required string ReleaseDatePrecision { get; set; }
    [JsonPropertyName("type")]
    public required string Type { get; set; }
    [JsonPropertyName("uri")]
    public required string Uri { get; set; }
    [JsonPropertyName("artists")]
    public required IEnumerable<AlbumSearchResponseArtist> Artists { get; set; }
}
