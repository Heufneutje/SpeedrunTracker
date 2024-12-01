namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record VariableValueContainer
{
    public Dictionary<string, VariableValue> Values { get; set; }

    public VariableValueContainer()
    {
        Values = [];
    }
}
