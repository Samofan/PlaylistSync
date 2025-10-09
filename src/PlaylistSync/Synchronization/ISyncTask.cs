using PlaylistSync.Synchronization.Models;

namespace PlaylistSync.Synchronization;

internal interface ISyncTask
{
    Task ExecuteAsync(SyncContext context, CancellationToken cancellationToken);
}
