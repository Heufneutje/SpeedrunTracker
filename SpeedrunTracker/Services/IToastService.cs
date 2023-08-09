using CommunityToolkit.Maui.Core;

namespace SpeedrunTracker.Services;

public interface IToastService
{
    Task ShowToastAsync(string message, ToastDuration duration = ToastDuration.Short, double fontSize = 14);
}
