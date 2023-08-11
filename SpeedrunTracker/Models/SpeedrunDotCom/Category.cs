namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Category : BaseSpeedrunObject
{
    public string Name { get; set; }
    public string Rules { get; set; }
    public bool Miscellaneous { get; set; }
    public CategoryType Type { get; set; }
    public BaseData<List<Variable>> Variables { get; set; }
}
