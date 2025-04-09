using APPLICATION.Helpers;

namespace APPLICATION.Abstractions.Pagination;

public interface IPaginationService
{
    Task<IPagedList<T>> PaginateAsync<T>(IQueryable<T> query, int page, int pageSize);
    IPagedList<T> PaginateList<T>(List<T> source, int take, int offset,int count);
    IPagedList<T> CreatePaginateResponse<T>(List<T> source, int take, int offset, int count);

}
