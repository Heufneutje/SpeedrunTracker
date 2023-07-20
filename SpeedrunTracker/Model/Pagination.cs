namespace SpeedrunTracker.Model;

public class Pagination
{
    public int Offset { get; set; }
    public int Max { get; set; }
    public int Size { get; set; }
    public List<PaginationLink> Links { get; set; }
}
