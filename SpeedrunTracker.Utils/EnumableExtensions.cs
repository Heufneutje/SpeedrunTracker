using System.Collections.ObjectModel;

namespace SpeedrunTracker.Utils;

public static class EnumableExtensions
{
    public static ObservableCollection<T> AsObservableCollection<T>(this IEnumerable<T> collection)
    {
        return new ObservableCollection<T>(collection);
    }
}