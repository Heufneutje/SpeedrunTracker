namespace SpeedrunTracker.ViewModels;

public abstract class BasePopupViewModel : BaseViewModel, IQueryAttributable
{
    public abstract void ApplyQueryAttributes(IDictionary<string, object> query);
}
