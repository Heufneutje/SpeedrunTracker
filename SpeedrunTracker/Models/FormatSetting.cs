namespace SpeedrunTracker.Models;

public class FormatSetting
{
    public string Description { get; set; }
    public string FormatString { get; set; }

    public FormatSetting(string description, string formatString)
    {
        Description = description;
        FormatString = formatString;
    }

    public override string ToString() => Description;
}
