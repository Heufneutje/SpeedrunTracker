namespace SpeedrunTracker.Models.Base;

public record BaseData<T>
{
    public required T Data { get; set; }
}
