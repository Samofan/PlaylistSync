using PlaylistSync.Models;

namespace PlaylistSync.Discogs.Models;

internal record DiscogsArtist(string Name) 
    : Artist(Name);