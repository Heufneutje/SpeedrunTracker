using Microsoft.Extensions.Configuration;
using Refit;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Pages;
using SpeedrunTracker.Repository;
using SpeedrunTracker.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpeedrunTracker.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSpeedrunTrackerServices(this IServiceCollection services, IConfiguration config)
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumMemberConverter()
            }
        };

        RefitSettings settings = new()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(options)
        };

        services.AddRefitClient<IGamesService>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddRefitClient<ILeaderboardService>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddScoped<IGamesRepository, GamesRepository>();
        services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();

        return services;
    }

    private static void ConfigureHttpClient(HttpClient client)
    {
        //client.BaseAddress = new Uri(config["speedrun-dot-com:api-base-address"]));
        client.BaseAddress = new Uri("https://www.speedrun.com/api/v1");
        client.DefaultRequestHeaders.Add("User-Agent", "Heufneutje-SpeedrunTracker/0.0.1");
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddSingleton<GameSearchViewModel>();
        services.AddTransient<GameDetailViewModel>();

        return services;
    }

    public static IServiceCollection RegisterPages(this IServiceCollection services)
    {
        services.AddSingleton<SearchPage>();
        services.AddTransient<GameDetailPage>();

        return services;
    }
}
