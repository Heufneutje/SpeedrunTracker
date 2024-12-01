namespace SpeedrunTracker.Models;

public class EmbeddableUrl
{
    public int Number { get; set; }
    public required string Url { get; set; }
    public required string EmbedUrl { get; set; }
    public EmbedType EmbedType { get; set; }

    public string IconSource => EmbedType switch
    {
        EmbedType.YouTube => "youtube",
        EmbedType.Twitch => "twitch",
        _ => "video",
    };

    public string VideoNumberDescription => $"Video {Number}";
}
