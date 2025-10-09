using PlaylistSync.Models;

namespace PlaylistSync.Streaming.Spotify.Models;

internal record SpotifyArtist(string Name) : Artist(Name);