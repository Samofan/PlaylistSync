using PlaylistSync.Models;

namespace PlaylistSync.Streaming;

internal interface IStreamingServiceConnector
{
    Task<Album?> SearchAlbumAsync(Album album, CancellationToken cancellationToken); 
}
