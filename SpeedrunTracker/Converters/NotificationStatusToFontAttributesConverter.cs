using System.Globalization;

namespace SpeedrunTracker.Converters
{
    public class NotificationStatusToFontAttributesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is NotificationStatusType type))
                throw new InvalidOperationException("The value must be a boolean");

            return type == NotificationStatusType.Unread ? FontAttributes.Bold | FontAttributes.Italic : FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
