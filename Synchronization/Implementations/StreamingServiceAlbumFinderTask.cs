using System;
using Microsoft.Extensions.Logging;
using PlaylistSync.Streaming;
using PlaylistSync.Streaming.Spotify.Models;
using PlaylistSync.Synchronization.Models;

namespace PlaylistSync.Synchronization.Implementations;

internal sealed class StreamingServiceAlbumFinderTask(ILogger<StreamingServiceAlbumFinderTask> logger, IStreamingServiceConnector streamingService) : ISyncTask
{
    public async Task ExecuteAsync(SyncContext context, CancellationToken cancellationToken)
    {
        var foundAlbums = context.Wantlist.Select(async album =>
        {
            logger.LogDebug("Searching for album: {Title} by {Artist}",
                album.Title,
                string.Join(", ", album.Artists.Select(a => a.Name)));

            var foundAlbum = await streamingService.SearchAlbumAsync(new AlbumSearchRequest
            {
                Title = album.Title,
                Artist = album.Artists.FirstOrDefault()?.Name ?? string.Empty
            }, cancellationToken);

            if (foundAlbum != null)
            {
                logger.LogDebug("Found album: {Title} ({Year}) by {Artists}",
                    foundAlbum.Title,
                    foundAlbum.Year,
                    string.Join(", ", foundAlbum.Artists.Select(a => a.Name)));
            }
            else
            {
                logger.LogWarning("Album not found: {Title} by {Artist}",
                    album.Title,
                    string.Join(", ", album.Artists.Select(a => a.Name)));
            }

            return foundAlbum;
        });

        await Task.WhenAll(foundAlbums);

        context.SearchResults = [.. foundAlbums.Select(t => t.Result)];
    }
}
