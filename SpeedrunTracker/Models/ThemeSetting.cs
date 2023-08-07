namespace SpeedrunTracker.Models;

public class ThemeSetting
{
    public string Description { get; set; }
    public AppTheme Theme { get; set; }

    public ThemeSetting(string description, AppTheme theme)
    {
        Description = description;
        Theme = theme;
    }

    public override string ToString() => Description;
}
