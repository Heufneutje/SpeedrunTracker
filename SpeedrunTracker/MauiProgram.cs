using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SpeedrunTracker.DependencyInjection;

namespace SpeedrunTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
#if ANDROID
        Microsoft.Maui.Handlers.SearchBarHandler.Mapper.AppendToMapping("FullWidth", (handler, control) =>
        {
            handler.PlatformView.MaxWidth = int.MaxValue;
        });
#endif

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSpeedrunTrackerServices(builder.Configuration);
        builder.Services.AddLocalStorage();
        builder.Services.RegisterViewModels();
        builder.Services.RegisterPages();

        return builder.Build();
    }
}
