namespace SpeedrunTracker.Utils;

public static class TimeSpanExtensions
{
    public static string ToShortForm(this TimeSpan t)
    {
        string formattedTime = "";
        if (t.Hours > 0)
            formattedTime = $"{formattedTime} {t.Hours:00}h";

        if (t.Minutes > 0)
            formattedTime = $"{formattedTime} {t.Minutes:00}m";

        formattedTime = $"{formattedTime} {t.Seconds:00}s";

        if (t.Milliseconds > 0)
            formattedTime = $"{formattedTime} {t.Milliseconds:000}ms";

        return formattedTime.Trim();
    }
}
