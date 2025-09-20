namespace SpeedrunTracker.Models.Base;

public abstract record BaseLink
{
    public required string Uri { get; set; }
    public string SecureUri => Uri?.Replace("http://", "https://") ?? string.Empty;
}
