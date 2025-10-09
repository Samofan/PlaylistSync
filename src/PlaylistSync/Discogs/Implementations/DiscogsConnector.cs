using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlaylistSync.Discogs.Models;
using PlaylistSync.Models;
using PlaylistSync.Models.Settings;

namespace PlaylistSync.Discogs.Implementations;

internal sealed class DiscogsConnector(ILogger<DiscogsConnector> logger, HttpClient discogsClient, IOptions<ApplicationSettings> options) : IDiscogsConnector
{
    public async Task<IEnumerable<Album>> LoadWantlistAsync(CancellationToken cancellationToken)
    {
        var username = options.Value.DiscogsSettings.Username;

        logger.LogInformation("Loading wantlist from Discogs for user {Username}", username);

        var response = await discogsClient.GetFromJsonAsync<WantlistResponse>($"/users/{username}/wants", cancellationToken);

        var wantlist = response?.Wants?.Select(want => new DiscogsAlbum(
            want.BasicInformation.Title,
            want.BasicInformation.Year,
            want.BasicInformation.Artists.Select(artist => new DiscogsArtist(artist.Name))
        )) ?? [];

        logger.LogInformation("Wantlist for user {Username} contains {Count} items.", username, wantlist.Count());

        return wantlist;
    }
}
