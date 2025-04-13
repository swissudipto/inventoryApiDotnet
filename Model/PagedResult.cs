using inventoryApiDotnet.Model;

public class PagedResult<T>
{
    private List<Purchase> purchases;
    private long totalRecords;
    private int page;

    public List<T> Items { get; set; }
    public long TotalCount { get; set; } 

    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public PagedResult(List<T> items, long count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}