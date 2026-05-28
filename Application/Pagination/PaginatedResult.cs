using Microsoft.EntityFrameworkCore;

namespace Application.Pagination
{
    public class PaginatedResult<T> where T : class
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public int PageSize { get; }
        public IReadOnlyList<T> Items { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedResult(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }

        public static async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<T>(items, count, pageIndex, pageSize);
        }
    }
}

