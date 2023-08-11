namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record VariableScope
{
    public VariableScopeType Type { get; set; }

    public string Level { get; set; }
}
