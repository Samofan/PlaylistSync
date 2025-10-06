namespace PlaylistSync.Models.Settings;

internal sealed class ApplicationSettings
{
    public required DiscogsSettings DiscogsSettings { get; set; }
    public required StreamingServiceSettings StreamingServiceSettings { get; set; }
}

internal sealed class DiscogsSettings
{
    public required string Token { get; set; }
    public required string Username { get; set; }
}

internal sealed class StreamingServiceSettings
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
}