using CommunityToolkit.Mvvm.Input;
using SpeedrunTracker.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

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

    private string _query;

    public string Query
    {
        get => _query;
        set
        {
            _query = value;
            NotifyPropertyChanged();
        }
    }

    public abstract string SearchTextPlaceholder { get; }

    public ICommand SearchCommand => new AsyncRelayCommand(SearchAsync, CanSearch);
    public ICommand NavigateToCommand => new AsyncRelayCommand<Entity>(NavigateToAsync);

    public BaseSearchEntityViewModel(IToastService toastService) : base(toastService)
    {
    }

    private async Task SearchAsync()
    {
        IsRunningBackgroundTask = true;

        try
        {
            List<Entity> data = await SearchEntitiesAsync();
            if (data != null)
                Entities = data.AsObservableCollection();
        }
        finally
        {
            IsRunningBackgroundTask = false;
        }
    }

    protected abstract Task<List<Entity>> SearchEntitiesAsync();

    protected abstract Task NavigateToAsync(Entity entity);

    public bool CanSearch() => !IsRunningBackgroundTask;
}
