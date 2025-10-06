namespace PlaylistSync.Common;

internal interface IApiClient
{
    Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken);
}
