using APPLICATION.Abstractions.Pagination;
using APPLICATION.Helpers;

namespace INFRAESTRUCTURE.Helpers;

public class PaginationService : IPaginationService
{
    public async Task<IPagedList<T>> PaginateAsync<T>(IQueryable<T> query, int page, int pageSize)
    {
        return await PagedList<T>.CreateFromQueryAsync(query, page, pageSize);
    }

    public IPagedList<T> PaginateList<T>(List<T> source, int take, int offset,int count)
    {
        return  PagedList<T>.CreateFromList(source, take, offset, count);
    }

    public IPagedList<T> CreatePaginateResponse<T>(List<T> source, int take, int offset, int count)
    {
        return PagedList<T>.CreatePaginateResponse(source, take, offset, count);
    }
}