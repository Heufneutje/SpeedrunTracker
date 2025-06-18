using SpeedrunTracker.Resources.Localization;

namespace SpeedrunTracker.Services;

public class DialogService : BaseService, IDialogService
{
    public Task ShowAlertAsync(string title, string message, string? cancel = null)
    {
        cancel ??= Translate(nameof(AppStrings.DialogOkButton));
        return GetMainPage()?.DisplayAlertAsync(title, message, cancel) ?? Task.CompletedTask;
    }

    public async Task<bool> ShowConfirmationAsync(
        string title,
        string message,
        string? accept = null,
        string? cancel = null
    )
    {
        accept ??= Translate(nameof(AppStrings.DialogYesButton));
        cancel ??= Translate(nameof(AppStrings.DialogNoButton));

        Page? mainPage = GetMainPage();
        return mainPage is not null && await mainPage.DisplayAlertAsync(title, message, accept, cancel);
    }

    private static Page? GetMainPage() => Application.Current?.Windows[0].Page;
}
