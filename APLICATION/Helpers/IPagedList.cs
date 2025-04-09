namespace APPLICATION.Helpers;

public interface IPagedList<T>
{
    List<T> Items { get; }
    int Page { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int TotalPages { get; }
    bool HasNextPage { get; }
    bool HasPreviousPage { get; }
}
