namespace SpeedrunTracker.Models.SpeedrunDotCom;

public class PagedData<T> : BaseData<T>
{
    public Pagination Pagination { get; set; }
}
