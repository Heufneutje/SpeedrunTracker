using System.Collections.ObjectModel;

namespace SpeedrunTracker.Utils;

public static class EnumableExtensions
{
    public static ObservableCollection<T> AsObservableCollection<T>(this IEnumerable<T> collection)
    {
        return new ObservableCollection<T>(collection);
    }

    public static bool SequenceEqualOrNull<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        if (first == null && second == null)
            return true;
        if (first != null && second != null && first.SequenceEqual(second))
            return true;

        return false;
    }
}
