namespace SpeedrunTracker.Models;

public class LanguageSetting
{
    public required string DisplayName { get; set; }
    public required string CultureCode { get; set; }

    public override string ToString() => DisplayName;
}
