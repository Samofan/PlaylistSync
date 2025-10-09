using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlaylistSync.Auth.Models;
using PlaylistSync.Models.Settings;

namespace PlaylistSync.Auth.Implementations;

internal sealed class OAuthDelegatingHandler(ILogger<OAuthDelegatingHandler> logger, IOAuthClient authClient, IOptions<ApplicationSettings>options) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var clientId = options.Value.StreamingServiceSettings.ClientId;
        var clientSecret = options.Value.StreamingServiceSettings.ClientSecret;

        var tokenResponse = await authClient.RequestTokenAsync(
            new ClientCredentialsRequest(clientId, clientSecret),
            cancellationToken);

        request.Headers.Authorization = new("Bearer", tokenResponse.AccessToken);

        logger.LogDebug("Added bearer token to request {RequestUri}", request.RequestUri);

        return await base.SendAsync(request, cancellationToken);
    }
}
