using Microsoft.EntityFrameworkCore;

namespace Routine.Api.Helpers
{
    /// <summary>
    /// PageList should be a subclass of List so that PageList can use List's functionalities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T>: List<T>
    {
        public int CurrentPage { get; private set; } //Current page's page number
        public int TotalPage { get; private set; }
        public int PageSize { get; private set; } //how many items in one page
        public int TotalCount { get; private set; } //Total records
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPage;

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static async Task<PageList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize) {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber-1) * pageSize).Take(pageSize).ToListAsync();
            return new PageList<T>(items, count, pageNumber, pageSize);

        }
    }
}
