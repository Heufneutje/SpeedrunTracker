﻿using System.Globalization;

namespace SpeedrunTracker.Converters;

public class InverseBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(bool))
            throw new InvalidOperationException("The target must be a boolean");

        return value is null || !(bool)value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
