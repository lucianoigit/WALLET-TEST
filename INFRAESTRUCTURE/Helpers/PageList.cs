using APPLICATION.Helpers;
using Microsoft.EntityFrameworkCore;

namespace INFRAESTRUCTURE.Helpers
{
    public class PagedList<TSource> : IPagedList<TSource>
    {
        public List<TSource> Items { get; set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;


        private PagedList(List<TSource> items, int offset, int take, int count)
        {
            Items = items;
            Page = offset;
            PageSize = take;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)take);
        }
        public static async Task<PagedList<TSource>> CreateFromQueryAsync(IQueryable<TSource> source, int offset, int take)
        {
            var totalCount = await source.CountAsync();

            var items = await source.Skip((offset - 1) * take).Take(take).ToListAsync();

            return new PagedList<TSource>(items, offset, take, totalCount);
        }

        public static PagedList<TSource> CreateFromList(List<TSource> source, int take, int offset, int count)
        {
            var totalCount = count;

            var items = source.Skip((offset - 1) * take).Take(take).ToList();

            return new PagedList<TSource>(items, offset, take, count);
        }

        public static PagedList<TSource> CreatePaginateResponse(List<TSource> source, int take, int offset, int count)
        {
            var totalCount = count;

            return new PagedList<TSource>(source, take, offset, totalCount);
        }
    }

}
