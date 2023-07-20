namespace SpeedrunTracker.Model;

public class PagedData<T> : BaseData<T>
{
    public Pagination Pagination { get; set; }
}
