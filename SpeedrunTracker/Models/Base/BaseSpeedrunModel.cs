using System.Text.Json.Serialization;

namespace SpeedrunTracker.Models.Base;

public abstract record BaseSpeedrunModel
{
    public string Id { get; set; }

    public string? Weblink { get; set; }

    public List<Link> Links { get; set; }

    [JsonIgnore]
    public virtual string DisplayName => Id;

    protected BaseSpeedrunModel()
    {
        Id = string.Empty;
        Links = [];
    }
}
