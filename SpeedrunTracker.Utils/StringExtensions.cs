using System.Text.RegularExpressions;

namespace SpeedrunTracker.Utils;

public static partial class StringExtensions
{
    private static Regex _urlRegex = GeberateUrlRegex();

    [GeneratedRegex(@"(?<!\()((http|https):\/\/[^\s\)]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex GeberateUrlRegex();

    public static string MarkdownifyUrls(this string input) => _urlRegex.Replace(input, $"[$1]($1)");
}
