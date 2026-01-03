using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RealEstateHub.Application.Common
{
    public class PaginatedList<T> 
    {
        public IReadOnlyList<T> Items { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList (IReadOnlyList<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            PageSize = pageSize < 1 ? 10 : pageSize;
            TotalPages = (int)Math.Ceiling( count / (double)pageSize);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static PaginatedList<T> Create(IEnumerable<T> source, int count, int pageIndex, int pageSize)
        {
            return new PaginatedList<T>(source.ToList(), count, pageIndex, pageSize);
        }
    }
}
