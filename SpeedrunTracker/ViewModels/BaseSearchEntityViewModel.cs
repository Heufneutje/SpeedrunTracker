using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseSearchEntityViewModel : BaseNetworkActionViewModel
{
    private ObservableCollection<Entity> _entities;

    public ObservableCollection<Entity> Entities
    {
        get => _entities;
        set
        {
            _entities = value;
            NotifyPropertyChanged();
        }
    }

    private string? _query;

    public string Query
    {
        get => _query ?? string.Empty;
        set
        {
            _query = value;
            NotifyPropertyChanged();
        }
    }

    public abstract string SearchTextPlaceholder { get; }

    public ICommand SearchCommand => new AsyncRelayCommand(SearchAsync, CanSearch);
    public ICommand NavigateToCommand => new AsyncRelayCommand<Entity>(NavigateToAsync);

    protected BaseSearchEntityViewModel(IToastService toastService, IPopupService popupService)
        : base(toastService, popupService)
    {
        _entities = [];
    }

    private async Task SearchAsync()
    {
        ShowActivityIndicator("Searching...");

        try
        {
            List<Entity> data = await SearchEntitiesAsync();
            if (data != null)
                Entities = data.AsObservableCollection();
        }
        finally
        {
            CloseActivityIndicator();
        }
    }

    protected abstract Task<List<Entity>> SearchEntitiesAsync();

    protected abstract Task NavigateToAsync(Entity? entity);

    public bool CanSearch() => !IsRunningBackgroundTask;
}
