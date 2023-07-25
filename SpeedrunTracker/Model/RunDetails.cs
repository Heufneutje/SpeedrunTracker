namespace SpeedrunTracker.Model;

public class RunDetails
{
    public int Place { get; set; }
    public Speedrun Run { get; set; }
    public Category Category { get; set; }
    public Level Level { get; set; }
    public GamePlatform Platform { get; set; }
    public List<RunVariable> Variables { get; set; }
    public GameAssets GameAssets { get; set; }
    public Ruleset Ruleset { get; set; }
    public User Examiner { get; set; }
}
