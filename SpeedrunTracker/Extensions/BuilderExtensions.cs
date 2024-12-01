using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;
using SpeedrunTracker.Services;
using SpeedrunTracker.Services.LocalStorage;
using SpeedrunTracker.Services.SpeedrunData;
using SpeedrunTracker.ViewModels;
using SpeedrunTracker.Views;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpeedrunTracker.Extensions;

public static class BuilderExtensions
{
    public static MauiApp BuildApp(this MauiAppBuilder builder)
    {
        IConfiguration config = AddConfiguration(builder);

        AddFramework(builder);
        AddPlatformSpecificHandlers();
        AddLogging(builder);
        AddRepositories(builder.Services, config);
        AddServices(builder.Services);
        AddLocalStorage(builder.Services);
        AddViewModels(builder.Services);
        AddPages(builder.Services);

        return builder.Build();
    }

    private static void AddFramework(MauiAppBuilder builder)
    {
        builder
           .UseMauiCommunityToolkit()
           .UseMauiApp<App>()
           .ConfigureFonts(fonts =>
           {
               fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
               fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
           });
    }

    private static void AddPlatformSpecificHandlers()
    {
#if ANDROID
        Microsoft.Maui.Handlers.SearchBarHandler.Mapper.AppendToMapping("FullWidth", (handler, control) =>
        {
            handler.PlatformView.MaxWidth = int.MaxValue;
        });
#endif
    }

    private static void AddLogging(MauiAppBuilder builder)
    {
#if DEBUG
        builder.Logging.AddDebug();
#endif
    }

    private static IConfiguration AddConfiguration(MauiAppBuilder builder)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream("SpeedrunTracker.appsettings.json") ??
            throw new FileNotFoundException("appsettings.json file is missing");
        
        IConfigurationRoot config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

        builder.Configuration.AddConfiguration(config);
        return config;
    }

    private static void AddRepositories(IServiceCollection services, IConfiguration config)
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

        void ConfigureHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri($"{config["speedrun-dot-com:base-address"]}{config["speedrun-dot-com:api-address"]}");
            client.DefaultRequestHeaders.Add("User-Agent", $"Heufneutje-SpeedrunTracker/{App.Version}");
        }
    }

    private static void AddServices(IServiceCollection services)
    {
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
    }

    private static void AddViewModels(IServiceCollection services)
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
    }

    private static void AddPages(IServiceCollection services)
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
    }

    private static void AddLocalStorage(IServiceCollection services)
    {
        services.AddTransient<IMauiInitializeService, LocalDatabaseInitializer>();
        services.AddSingleton<ILocalDatabaseService, LocalDatabaseService>();
        services.AddSingleton<ICacheDatabaseService, CacheDatabaseService>();
        services.AddSingleton<ILocalFollowService, LocalFollowService>();
        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
        services.AddSingleton<ICacheService, CacheService>();
    }
}
