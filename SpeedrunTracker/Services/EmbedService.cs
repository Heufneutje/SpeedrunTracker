using System.Collections.Specialized;
using System.Web;

namespace SpeedrunTracker.Services;

public class EmbedService : IEmbedService
{
    public List<EmbeddableUrl> GetEmbeddableUrls(SpeedrunVideos videos)
    {
        List<EmbeddableUrl> urls = new();
        foreach (string uri in videos.Links.Select(x => x.Uri))
        {
            if (uri.Contains("youtube") || uri.Contains("youtu.be"))
                urls.Add(HandleYouTube(uri));
            else if (uri.Contains("twitch"))
                urls.Add(HandleTwitch(uri));
            else
                urls.Add(HandleUnknown(uri));
        }

        if (urls.Count > 1)
            urls.ForEach(x => x.Number = urls.IndexOf(x) + 1);

        return urls;
    }

    private EmbeddableUrl HandleYouTube(string url)
    {
        Uri uri = new(url);
        NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
        string? videoId = query.AllKeys.Contains("v") ? query["v"] : uri.Segments[^1];
        return new EmbeddableUrl()
        {
            Url = url,
            EmbedUrl = $"https://www.youtube.com/embed/{videoId}",
            EmbedType = EmbedType.Unknown
        };
    }

    private EmbeddableUrl HandleTwitch(string url)
    {
        Uri uri = new(url);
        string videoId = uri.Segments[^1];
        return new EmbeddableUrl()
        {
            Url = url,
            EmbedUrl = $"https://player.twitch.tv/?autoplay=false&enableExtensions=true&parent=speedruntracker&video={videoId}",
            EmbedType = EmbedType.Twitch
        };
    }

    private EmbeddableUrl HandleUnknown(string url)
    {
        return new EmbeddableUrl()
        {
            Url = url,
            EmbedUrl = url,
            EmbedType = EmbedType.Unknown
        };
    }
}
