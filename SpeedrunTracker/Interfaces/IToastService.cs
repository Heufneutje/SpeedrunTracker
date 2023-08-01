using CommunityToolkit.Maui.Core;

namespace SpeedrunTracker.Interfaces;

public interface IToastService
{
    Task ShowToastAsync(string message, ToastDuration duration = ToastDuration.Short, double fontSize = 14);
}
