using System.Collections.Generic;

namespace Core.Pagination
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pagSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PagSize = pagSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PagSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}