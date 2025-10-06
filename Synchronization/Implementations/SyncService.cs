using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlaylistSync.Streaming;
using PlaylistSync.Streaming.Spotify.Models;
using PlaylistSync.Synchronization.Models;

namespace PlaylistSync.Synchronization.Implementations;

internal sealed class SyncService(ILogger<SyncService> logger,
    [FromKeyedServices("WantlistLoader")] ISyncTask wantlistLoader,
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

        var albumToSearch = context.Wantlist.First();
        var foundAlbum = await streamingServiceConnector.SearchAlbumAsync(new AlbumSearchRequest
        {
            Title = albumToSearch.Title,
            Artist = albumToSearch.Artists.FirstOrDefault()?.Name ?? string.Empty
        }, cancellationToken);

        var pinkFloyd = await streamingServiceConnector.SearchAlbumAsync(new AlbumSearchRequest
        {
            Title = "The Dark Side of the Moon",
            Artist = "Pink Floyd"
        }, cancellationToken);

        logger.LogInformation("Found album: {Title} ({Year}) by {Artists}",
            foundAlbum?.Title,
            foundAlbum?.Year,
            string.Join(", ", foundAlbum?.Artists.Select(a => a.Name) ?? []));
    }
}