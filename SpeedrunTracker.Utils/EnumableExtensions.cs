﻿using System.Collections.ObjectModel;

namespace SpeedrunTracker.Utils;

public static class EnumableExtensions
{
    public static ObservableCollection<T> AsObservableCollection<T>(this IEnumerable<T> collection)
    {
        return new ObservableCollection<T>(collection);
    }

    public static bool SequenceEqualOrNull<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        if (first is null && second is null)
            return true;
        if (first is not null && second is not null && first.SequenceEqual(second))
            return true;

        return false;
    }
}
