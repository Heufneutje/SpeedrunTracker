using Microsoft.Extensions.Configuration;
using Refit;
using SpeedrunTracker.Interfaces;
using SpeedrunTracker.Repository;
using SpeedrunTracker.Services;
using SpeedrunTracker.ViewModels;
using SpeedrunTracker.Views;
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
        services.AddRefitClient<IUserService>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddRefitClient<INotificationService>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddScoped<IGamesRepository, GamesRepository>();
        services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddSingleton<IBrowserService, BrowserService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IToastService, ToastService>();

        return services;

        void ConfigureHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri(config["speedrun-dot-com:api-base-address"]);
            client.DefaultRequestHeaders.Add("User-Agent", "Heufneutje-SpeedrunTracker/0.0.1");
        }
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddSingleton<GameSearchViewModel>();
        services.AddSingleton<UserSearchViewModel>();
        services.AddSingleton<FollowedEntityViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<ProfileViewModel>();
        services.AddSingleton<NotificationListViewModel>();
        services.AddTransient<GameDetailViewModel>();
        services.AddTransient<RunDetailsViewModel>();
        services.AddTransient<UserDetailsViewModel>();

        return services;
    }

    public static IServiceCollection RegisterPages(this IServiceCollection services)
    {
        services.AddSingleton<GameSearchPage>();
        services.AddSingleton<UserSearchPage>();
        services.AddSingleton<FollowingPage>();
        services.AddSingleton<SettingsPage>();
        services.AddSingleton<ProfilePage>();
        services.AddSingleton<NotificationsPage>();
        services.AddTransient<GameDetailPage>();
        services.AddTransient<RunDetailsPage>();
        services.AddTransient<UserDetailPage>();

        return services;
    }

    public static IServiceCollection AddLocalStorage(this IServiceCollection services)
    {
        services.AddTransient<IMauiInitializeService, LocalDatabaseInitializer>();
        services.AddSingleton<ILocalDatabaseService, LocalDatabaseService>();
        services.AddSingleton<ILocalFollowService, LocalFollowService>();
        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();

        return services;
    }
}
