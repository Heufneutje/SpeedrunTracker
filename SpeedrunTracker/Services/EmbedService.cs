using System.Collections.Specialized;
using System.Web;

namespace SpeedrunTracker.Services;

public class EmbedService : IEmbedService
{
    public List<EmbeddableUrl> GetEmbeddableUrls(SpeedrunVideos videos)
    {
        List<EmbeddableUrl> urls = new List<EmbeddableUrl>();
        foreach (Link link in videos.Links)
        {
            if (link.Uri.Contains("youtube") || link.Uri.Contains("youtu.be"))
                urls.Add(HandleYouTube(link.Uri));
            else if (link.Uri.Contains("twitch"))
                urls.Add(HandleTwitch(link.Uri));
            else
                urls.Add(HandleUnknown(link.Uri));
        }

        if (urls.Count > 1)
            urls.ForEach(x => x.Number = urls.IndexOf(x) + 1);

        return urls;
    }

    private EmbeddableUrl HandleYouTube(string url)
    {
        Uri uri = new(url);
        NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
        string videoId = query.AllKeys.Contains("v") ? query["v"] : uri.Segments.Last();
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
        string videoId = uri.Segments.Last();
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
