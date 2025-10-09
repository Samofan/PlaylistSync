using PlaylistSync.Models;
using PlaylistSync.Streaming.Spotify.Models;

namespace PlaylistSync.Streaming;

internal interface IStreamingServiceConnector
{
    Task<Album?> SearchAlbumAsync(AlbumSearchRequest albumSearchRequest, CancellationToken cancellationToken); 
}
