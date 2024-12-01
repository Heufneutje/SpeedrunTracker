namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record Category : BaseSpeedrunModel, INamedSpeedrunModel
{
    public required string Name { get; set; }
    public string? Rules { get; set; }
    public bool Miscellaneous { get; set; }
    public CategoryType Type { get; set; }
    public BaseData<List<Variable>> Variables { get; set; }

    public Category()
    {
        Variables = new BaseData<List<Variable>>() { Data = [] };
    }
}
