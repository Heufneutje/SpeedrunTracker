namespace SpeedrunTracker.Model;

public class Leaderboard
{
    public string Weblink { get; set; }
    public List<LeaderboardEntry> Runs { get; set; }
    public BaseData<List<User>> Players { get; set; }
}
