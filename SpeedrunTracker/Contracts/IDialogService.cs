namespace SpeedrunTracker.Contracts;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string cancel = "OK");

    Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No");
}
