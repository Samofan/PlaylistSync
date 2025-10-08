using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlaylistSync.Streaming;
using PlaylistSync.Streaming.Spotify.Models;
using PlaylistSync.Synchronization.Models;

namespace PlaylistSync.Synchronization.Implementations;

internal sealed class SyncService(ILogger<SyncService> logger,
    [FromKeyedServices(nameof(WantlistLoaderTask))] ISyncTask wantlistLoader,
    [FromKeyedServices(nameof(StreamingServiceAlbumFinderTask))] ISyncTask albumFinder,
    IStreamingServiceConnector streamingServiceConnector)
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

        await albumFinder.ExecuteAsync(context, cancellationToken);

        foreach (var album in context.SearchResults
            .Where(a => a is not null)
            .OrderBy(a => a!.Artists.FirstOrDefault()?.Name)
            .ThenBy(a => a!.Year))
        {
            logger.LogInformation("Found Album: {Title} ({Year}) by {Artists} with id {Id}",
                album!.Title,
                album.Year,
                string.Join(", ", album.Artists.Select(a => a.Name)),
                album is SpotifyAlbum spotifyAlbum ? spotifyAlbum.Id : "N/A");
        }
    }
}