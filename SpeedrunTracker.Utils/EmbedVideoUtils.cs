using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;

namespace SpeedrunTracker.Utils;

public static class EmbedVideoUtils
{
    public static string GetEmbedUrl(string url)
    {
        if (url.Contains("youtube"))
        {
            Uri uri = new(url);
            NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
            string? videoId = query.AllKeys.Contains("v") ? query["v"] : uri.Segments.Last();
            return $"https://www.youtube.com/embed/{videoId}";
        }
        else if (url.Contains("twitch"))
        {
            Uri uri = new(url);
            string videoId = uri.Segments.Last();
            Debug.WriteLine(videoId);
            return $"https://player.twitch.tv/?autoplay=false&enableExtensions=true&parent=speedruntracker&video={videoId}";
        }

        return url;
    }
}
