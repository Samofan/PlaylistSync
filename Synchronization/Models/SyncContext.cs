using PlaylistSync.Models;

namespace PlaylistSync.Synchronization.Models;

internal sealed class SyncContext
{
    public IEnumerable<Album> Wantlist { get; set; } = [];
}
