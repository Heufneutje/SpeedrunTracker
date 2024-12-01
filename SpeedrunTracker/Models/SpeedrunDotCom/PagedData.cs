namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record PagedData<T> : BaseData<T>
{
    public required Pagination Pagination { get; set; }
}
