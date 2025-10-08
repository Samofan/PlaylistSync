using PlaylistSync.Models;

namespace PlaylistSync.Streaming.Spotify.Models;

internal record SpotifyAlbum(
    string Id,
    string Title,
    int Year,
    IEnumerable<Artist> Artists) : Album(Title, Year, Artists);
