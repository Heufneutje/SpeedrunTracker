using SQLite;

namespace SpeedrunTracker.Models.LocalData;

public class UserSettings
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int MaxLeaderboardResults { get; set; }
}
