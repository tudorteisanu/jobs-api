public class PagedResult<T>
{
    public List<T> Data { get; set; }
    public int TotalCount { get; set; }
    public int Size { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }

    public PagedResult(List<T> data, int count, int pageNumber, int pageSize)
    {
        Data = data;
        TotalCount = count;
        Size = pageSize;
        Page = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
}
