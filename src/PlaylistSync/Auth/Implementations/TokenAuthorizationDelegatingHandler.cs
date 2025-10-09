using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlaylistSync.Models.Settings;

namespace PlaylistSync.Auth.Implementations;

internal sealed class TokenAuthorizationDelegatingHandler(ILogger<TokenAuthorizationDelegatingHandler> logger, IOptions<ApplicationSettings> options) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = options.Value.DiscogsSettings.Token;

        request.Headers.Add("Authorization", $"Discogs token={token}");

        logger.LogDebug("Added Discogs token to request {RequestUri}", request.RequestUri);

        return await base.SendAsync(request, cancellationToken);
    }
}
