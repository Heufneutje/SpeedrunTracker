﻿using Microsoft.Extensions.Configuration;
using Refit;
using SpeedrunTracker.Services;
using SpeedrunTracker.Services.LocalStorage;
using SpeedrunTracker.Services.SpeedrunData;
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

        services.AddRefitClient<IGameRepository>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddRefitClient<ILeaderboardRepository>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddRefitClient<IUserRepository>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddRefitClient<INotificationRepository>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddRefitClient<IGameSeriesRepository>(settings).ConfigureHttpClient(ConfigureHttpClient);
        services.AddSingleton<IGameService, GameService>();
        services.AddSingleton<ILeaderboardService, LeaderboardService>();
        services.AddSingleton<IUserService, UserService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IGameSeriesService, GameSeriesService>();
        services.AddSingleton<IBrowserService, BrowserService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IToastService, ToastService>();
        services.AddSingleton<IJsonSerializationService, JsonSerializationService>();
        services.AddSingleton<IEmbedService, EmbedService>();
        services.AddSingleton<IShareService, ShareService>();

        return services;

        void ConfigureHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri($"{config["speedrun-dot-com:base-address"]}{config["speedrun-dot-com:api-address"]}");
            client.DefaultRequestHeaders.Add("User-Agent", $"Heufneutje-SpeedrunTracker/{App.Version}");
        }
    }

    public static IServiceCollection RegisterViewModels(this IServiceCollection services)
    {
        services.AddSingleton<GameSearchViewModel>();
        services.AddSingleton<GameSeriesSearchViewModel>();
        services.AddSingleton<UserSearchViewModel>();
        services.AddSingleton<FollowedEntityViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<ProfileViewModel>();
        services.AddSingleton<NotificationListViewModel>();
        services.AddSingleton<AboutViewModel>();
        services.AddTransient<GameDetailViewModel>();
        services.AddTransient<RunDetailsViewModel>();
        services.AddTransient<UserDetailsViewModel>();
        services.AddTransient<GameSeriesDetailViewModel>();

        return services;
    }

    public static IServiceCollection RegisterPages(this IServiceCollection services)
    {
        services.AddSingleton<GameSearchPage>();
        services.AddSingleton<GameSeriesSearchPage>();
        services.AddSingleton<UserSearchPage>();
        services.AddSingleton<FollowingPage>();
        services.AddSingleton<SettingsPage>();
        services.AddSingleton<ProfilePage>();
        services.AddSingleton<NotificationsPage>();
        services.AddSingleton<AboutPage>();
        services.AddTransient<GameDetailPage>();
        services.AddTransient<RunDetailsPage>();
        services.AddTransient<UserDetailPage>();
        services.AddTransient<GameSeriesDetailsPage>();

        return services;
    }

    public static IServiceCollection AddLocalStorage(this IServiceCollection services)
    {
        services.AddTransient<IMauiInitializeService, LocalDatabaseInitializer>();
        services.AddSingleton<ILocalDatabaseService, LocalDatabaseService>();
        services.AddSingleton<ICacheDatabaseService, CacheDatabaseService>();
        services.AddSingleton<ILocalFollowService, LocalFollowService>();
        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
