namespace SpeedrunTracker.Utils;

public static class NumericExtensions
{
    public static string AsOrdinal(this int number)
    {
        if (number <= 0)
            return number.ToString();

        return (number % 100) switch
        {
            11 or 12 or 13 => number + "th",
            _ => (number % 10) switch
            {
                1 => number + "st",
                2 => number + "nd",
                3 => number + "rd",
                _ => number + "th",
            },
        };
    }
}
