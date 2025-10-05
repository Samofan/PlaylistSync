using System;
using PlaylistSync.Models;

namespace PlaylistSync.Discogs;

internal interface IDiscogsConnector
{
    Task<IEnumerable<Album>> LoadWantlistAsync(CancellationToken cancellationToken);
}
