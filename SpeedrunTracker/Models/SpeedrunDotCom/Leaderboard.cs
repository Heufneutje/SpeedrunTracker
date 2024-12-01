namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Leaderboard
{
    public required string Weblink { get; set; }
    public List<LeaderboardEntry> Runs { get; set; }
    public BaseData<List<User>> Players { get; set; }

    public Leaderboard()
    {
        Runs = [];
        Players = new BaseData<List<User>> { Data = [] };
    }
}
