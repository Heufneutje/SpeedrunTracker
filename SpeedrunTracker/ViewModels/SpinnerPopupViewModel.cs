namespace SpeedrunTracker.ViewModels;

public class SpinnerPopupViewModel : BaseViewModel
{
    private string _loadingText = "Loading...";

    public string LoadingText
    {
        get => _loadingText;
        set
        {
            _loadingText = value;
            NotifyPropertyChanged();
        }
    }
}
