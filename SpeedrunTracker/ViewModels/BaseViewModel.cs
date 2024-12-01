using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpeedrunTracker.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    private bool _isRunningBackgroundTask;

    public bool IsRunningBackgroundTask
    {
        get => _isRunningBackgroundTask;
        set
        {
            _isRunningBackgroundTask = value;
            NotifyPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
