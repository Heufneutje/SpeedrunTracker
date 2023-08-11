namespace SpeedrunTracker.Models.Base;

public record BaseData<T>
{
    public T Data { get; set; }
}
