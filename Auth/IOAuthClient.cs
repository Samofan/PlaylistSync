using System;

namespace PlaylistSync.Auth;

public interface IOAuthClient
{
    Task<ClientCredentialsTokenResponse> RequestTokenAsync(ClientCredentialsRequest clientCredentialsRequest, CancellationToken cancellationToken = default);

    Task<AuthorizationCodeTokenResponse> RequestTokenAsync(AuthorizationCodeRequest authorizationCodeRequest, CancellationToken cancellationToken = default);
}
