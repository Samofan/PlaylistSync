using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlaylistSync.Discogs;
using PlaylistSync.Discogs.Implementations;
using PlaylistSync.Models.Settings;
using PlaylistSync.Synchronization;
using PlaylistSync.Synchronization.Implementations;
using Serilog;
using Microsoft.Extensions.Options;
using PlaylistSync.Streaming.Spotify;
using PlaylistSync.Auth;
using PlaylistSync.Streaming;
using PlaylistSync.Auth.Implementations;
using PlaylistSync.Common;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddUserSecrets<ApplicationSettings>();
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(nameof(ApplicationSettings)));

var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

builder.Services.AddLogging();
builder.Services.AddSerilog(logger);
builder.Services.AddMemoryCache();

builder.Services.ConfigureHttpClientDefaults(configuration =>
{
    configuration.AddStandardResilienceHandler();
});

builder.Services.AddTransient<OAuthDelegatingHandler>();
builder.Services.AddTransient<TokenAuthorizationDelegatingHandler>();

builder.Services.AddHttpClient<IDiscogsConnector, DiscogsConnector>((serviceProvider, client) =>
{
    client.BaseAddress = new Uri("https://api.discogs.com/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("PlaylistSync/0.1");
}).AddHttpMessageHandler<TokenAuthorizationDelegatingHandler>();

builder.Services.AddHttpClient<IOAuthClient, SpotifyOAuthClient>();
builder.Services.AddHttpClient<SpotifyApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.spotify.com/v1/");
}).AddHttpMessageHandler<OAuthDelegatingHandler>();
builder.Services.AddScoped<IStreamingServiceConnector, SpotifyConnector>();

builder.Services.AddKeyedScoped<ISyncTask, WantlistLoaderTask>("WantlistLoader");

builder.Services.AddScoped<SyncService>();

var app = builder.Build();

var syncService = app.Services.GetRequiredService<SyncService>();
await syncService.StartAsync(CancellationToken.None);
