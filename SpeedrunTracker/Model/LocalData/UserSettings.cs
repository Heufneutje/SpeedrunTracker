using SQLite;

namespace SpeedrunTracker.Model.LocalData;

public class UserSettings
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public bool EnableGameSearch { get; set; }
    public bool EnableUserSearch { get; set; }
    public int MaxLeaderboardResults { get; set; }
}
