using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Helpers
{
    public class PaginatedList<T>: List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize )
        {
            PageIndex = pageIndex;
            TotalCount = count;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);

            this.AddRange(items);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}