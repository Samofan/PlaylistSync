using PlaylistSync.Models;

namespace PlaylistSync.Discogs.Models;

internal record DiscogsAlbum(string Title, int Year, IEnumerable<Artist> Artists) 
    : Album(Title, Year, Artists);
