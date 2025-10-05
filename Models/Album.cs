namespace PlaylistSync.Models;

internal record Album(string Title, int Year, IEnumerable<Artist> Artists);
