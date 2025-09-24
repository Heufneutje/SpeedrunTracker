namespace SpeedrunTracker.Contracts;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string? cancel = null);

    Task<bool> ShowConfirmationAsync(string title, string message, string? accept = null, string? cancel = null);
}
