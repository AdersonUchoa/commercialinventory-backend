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

        public PaginatedResult(IReadOnlyList<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }
    }
}

