namespace SPA.Web.Models;

public class PaginationResultViewModel<T>
{
    public IEnumerable<T> Data { get; set; } = null!;
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
}