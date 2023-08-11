namespace SpeedrunTracker.Models.SpeedrunDotCom;

public record PagedData<T> : BaseData<T>
{
    public Pagination Pagination { get; set; }
}
