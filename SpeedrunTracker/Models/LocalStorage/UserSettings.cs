using SQLite;

namespace SpeedrunTracker.Models.LocalStorage;

public class UserSettings
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public AppTheme Theme { get; set; }
    public int MaxLeaderboardResults { get; set; }
    public string? DateFormat { get; set; }
    public string? TimeFormat { get; set; }
}
