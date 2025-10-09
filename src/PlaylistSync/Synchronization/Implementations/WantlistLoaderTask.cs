using PlaylistSync.Discogs;
using PlaylistSync.Synchronization.Models;

namespace PlaylistSync.Synchronization.Implementations;

internal sealed class WantlistLoaderTask(IDiscogsConnector discogsConnector) : ISyncTask
{
    public async Task ExecuteAsync(SyncContext context, CancellationToken cancellationToken)
    {
        var wantlist = await discogsConnector.LoadWantlistAsync(cancellationToken);

        context.Wantlist = wantlist;
    }
}
