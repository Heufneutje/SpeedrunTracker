namespace SpeedrunTracker.Model;

public class Game : BaseSpeedrunObject
{
    public Names Names { get; set; }
    public int Released { get; set; }
    public GameAssets Assets { get; set; }
    public BaseData<List<Platform>> Platforms { get; set; }
}
