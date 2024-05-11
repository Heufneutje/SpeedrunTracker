using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace SpeedrunTracker.Services;

public class ToastService : IToastService
{
    public async Task ShowToastAsync(string message, ToastDuration duration = ToastDuration.Short, double fontSize = 14)
    {
        using CancellationTokenSource cancellationTokenSource = new();
        IToast toast = Toast.Make(message, duration, fontSize);
        await toast.Show(cancellationTokenSource.Token);
    }
}
