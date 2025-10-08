namespace PlaylistSync.Models;

internal abstract record Album(string Title, int Year, IEnumerable<Artist> Artists);
