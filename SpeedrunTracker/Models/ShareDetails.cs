namespace SpeedrunTracker.Models;

public class ShareDetails
{
    public string Uri { get; set; }
    public string Title { get; set; }

    public ShareDetails(string uri, string title)
    {
        Uri = uri;
        Title = title;
    }
}
