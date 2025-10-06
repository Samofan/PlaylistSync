using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlaylistSync.Synchronization.Models;

namespace PlaylistSync.Synchronization.Implementations;

internal sealed class SyncService(ILogger<SyncService> logger,
    [FromKeyedServices("WantlistLoader")] ISyncTask wantlistLoader)
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("SyncService is starting.");

        var context = new SyncContext();

        await wantlistLoader.ExecuteAsync(context, cancellationToken);

        foreach (var album in context.Wantlist
            .OrderBy(a => a.Artists.FirstOrDefault()?.Name)
            .ThenBy(a => a.Year))
        {
            logger.LogDebug("Album: {Title} ({Year}) by {Artists}",
                album.Title,
                album.Year,
                string.Join(", ", album.Artists.Select(a => a.Name)));
        }
    }
}