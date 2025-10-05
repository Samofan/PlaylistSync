namespace PlaylistSync.Models.Settings;

internal sealed class ApplicationSettings
{
    public required DiscogsSettings DiscogsSettings { get; set; }
}

internal sealed class DiscogsSettings
{
    public required string Token { get; set; }
    public required string Username { get; set; }
}
