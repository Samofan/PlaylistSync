using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using PlaylistSync.Auth.Models;
using PlaylistSync.Streaming.Spotify;
using Rhino.Mocks.Constraints;
using Shouldly;

namespace PlaylistSync.UnitTests.Streaming.Spotify;

public class SpotifyOAuthClientTests
{
    private readonly Mock<ILogger<SpotifyOAuthClient>> _loggerMock;
    private readonly Mock<HttpClient> _httpClientMock;
    private readonly IMemoryCache _cache;

    private const string CacheKey = "Spotify:ClientCredentialsToken";

    public SpotifyOAuthClientTests()
    {
        _loggerMock = new Mock<ILogger<SpotifyOAuthClient>>();
        _httpClientMock = new Mock<HttpClient>();
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    [Fact]
    public async Task GIVEN_ValidCachedToken_WHEN_RequestingToken_THEN_ReturnsCachedToken()
    {
        // ARRANGE
        var cachedToken = new ClientCredentialsTokenResponse
        {
            AccessToken = string.Empty,
            TokenType = string.Empty,
            ExpiresIn = 3600
        };

        _cache.Set(CacheKey, cachedToken);

        var tokenRequest = new ClientCredentialsRequest(string.Empty, string.Empty);
        var cancellationToken = CancellationToken.None;

        var sut = new SpotifyOAuthClient(_loggerMock.Object, _httpClientMock.Object, _cache);

        // ACT
        var result = await sut.RequestTokenAsync(tokenRequest, cancellationToken);

        // ASSERT
        result.ShouldBe(cachedToken);
        _httpClientMock.Invocations.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GIVEN_InvalidCachedToken_WHEN_RequestingToken_THEN_RequestsNewToken()
    {
        // ARRANGE
        var cachedToken = new ClientCredentialsTokenResponse
        {
            AccessToken = string.Empty,
            TokenType = string.Empty,
            // Expires in 30 seconds.
            ExpiresIn = 30
        };

        var requestedToken = new ClientCredentialsTokenResponse
        {
            AccessToken = string.Empty,
            TokenType = string.Empty,
            ExpiresIn = 3600
        };

        var cancellationToken = CancellationToken.None;

        var httpResponse = new HttpResponseMessage
        {
            Content = JsonContent.Create(requestedToken),
            StatusCode = HttpStatusCode.OK
        };
        
        _httpClientMock
            .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), cancellationToken))
            .ReturnsAsync(httpResponse);

        _cache.Set(CacheKey, cachedToken);

        var tokenRequest = new ClientCredentialsRequest(string.Empty, string.Empty);

        var sut = new SpotifyOAuthClient(_loggerMock.Object, _httpClientMock.Object, _cache);

        // ACT
        var result = await sut.RequestTokenAsync(tokenRequest, cancellationToken);

        // ASSERT
        result.ShouldNotBe(cachedToken);
        result.ExpiresIn.ShouldBe(requestedToken.ExpiresIn);
        _httpClientMock.Invocations.Count.ShouldBe(1);
        _cache.Get<ClientCredentialsTokenResponse>(CacheKey)!.ExpiresIn.ShouldBe(requestedToken.ExpiresIn);
    }
}
