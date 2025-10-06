namespace PlaylistSync.Streaming.Spotify.Models;

internal class AlbumSearchRequest
{
    public string Title { get; init; } = string.Empty;
    public string Artist { get; init; } = string.Empty;
}