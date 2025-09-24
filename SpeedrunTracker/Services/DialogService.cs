using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.Services;

public class DialogService : IDialogService
{
    public Task ShowAlertAsync(string title, string message, string? cancel = null)
    {
        cancel ??= AppStrings.DialogOkButton;
        return GetMainPage()?.DisplayAlert(title, message, cancel) ?? Task.CompletedTask;
    }

    public async Task<bool> ShowConfirmationAsync(
        string title,
        string message,
        string? accept = null,
        string? cancel = null
    )
    {
        accept ??= AppStrings.DialogYesButton;
        cancel ??= AppStrings.DialogNoButton;

        Page? mainPage = GetMainPage();
        return mainPage is not null && await mainPage.DisplayAlert(title, message, accept, cancel);
    }

    private static Page? GetMainPage() => Application.Current?.Windows[0].Page;
}
