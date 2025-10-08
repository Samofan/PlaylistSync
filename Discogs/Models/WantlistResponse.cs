using System.Text.Json.Serialization;

namespace PlaylistSync.Discogs.Models;

/// <summary>
/// Root object for the wantlist API response.
/// </summary>
internal sealed class WantlistResponse
{
    [JsonPropertyName("pagination")]
    public required PaginationInfo Pagination { get; set; }

    [JsonPropertyName("wants")]
    public required IEnumerable<Want> Wants { get; set; }
}

/// <summary>
/// Contains pagination information for the result set.
/// </summary>
internal sealed class PaginationInfo
{
    [JsonPropertyName("per_page")]
    public required int PerPage { get; set; }

    [JsonPropertyName("pages")]
    public required int Pages { get; set; }

    [JsonPropertyName("page")]
    public required int Page { get; set; }

    [JsonPropertyName("items")]
    public required int Items { get; set; }
}

/// <summary>
/// Represents a single item in the wantlist.
/// </summary>
internal sealed class Want
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("resource_url")]
    public required string ResourceUrl { get; set; }

    [JsonPropertyName("rating")]
    public required int Rating { get; set; }
    
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("basic_information")]
    public required BasicInformation BasicInformation { get; set; }
}

/// <summary>
/// Contains basic information about a release.
/// </summary>
internal sealed class BasicInformation
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("resource_url")]
    public required string ResourceUrl { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("year")]
    public required int Year { get; set; }

    [JsonPropertyName("thumb")]
    public required string Thumb { get; set; }

    [JsonPropertyName("cover_image")]
    public required string CoverImage { get; set; }

    [JsonPropertyName("formats")]
    public required IEnumerable<Format> Formats { get; set; }

    [JsonPropertyName("labels")]
    public required IEnumerable<Label> Labels { get; set; }

    [JsonPropertyName("artists")]
    public required IEnumerable<WantlistArtist> Artists { get; set; }
}

/// <summary>
/// Describes the format of a release (e.g., CD, Vinyl).
/// </summary>
internal sealed class Format
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("qty")]
    public required string Qty { get; set; }
    
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("descriptions")]
    public required IEnumerable<string> Descriptions { get; set; }
}

/// <summary>
/// Represents a record label.
/// </summary>
internal sealed class Label
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("catno")]
    public required string CatNo { get; set; }

    [JsonPropertyName("entity_type")]
    public required string EntityType { get; set; }

    [JsonPropertyName("resource_url")]
    public required string ResourceUrl { get; set; }
}

/// <summary>
/// Represents an artist.
/// </summary>
internal sealed class WantlistArtist
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("join")]
    public required string Join { get; set; }

    [JsonPropertyName("anv")]
    public required string Anv { get; set; }

    [JsonPropertyName("tracks")]
    public required string Tracks { get; set; }

    [JsonPropertyName("role")]
    public required string Role { get; set; }

    [JsonPropertyName("resource_url")]
    public required string ResourceUrl { get; set; }
}
