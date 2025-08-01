using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SpeedrunTracker.ViewModels;

public abstract partial class BaseSearchEntityViewModel : BaseNetworkActionViewModel
{
    [ObservableProperty]
    private ObservableCollection<Entity> _entities;

    [ObservableProperty]
    private string? _query;

    [ObservableProperty]
    protected Entity? _selectedEntity;

    public abstract string SearchTextPlaceholder { get; }

    protected BaseSearchEntityViewModel(IToastService toastService, IPopupService popupService)
        : base(toastService, popupService)
    {
        _entities = [];
    }

    [RelayCommand(CanExecute = nameof(CanSearch))]
    private async Task SearchAsync()
    {
        ShowActivityIndicator("Searching...");

        try
        {
            List<Entity> data = await SearchEntitiesAsync();
            if (data is not null)
                Entities = data.AsObservableCollection();
        }
        finally
        {
            CloseActivityIndicator();
        }
    }
   
    protected abstract Task<List<Entity>> SearchEntitiesAsync();

    [RelayCommand]
    protected abstract Task NavigateToAsync();

    public bool CanSearch() => !IsRunningBackgroundTask;
}
