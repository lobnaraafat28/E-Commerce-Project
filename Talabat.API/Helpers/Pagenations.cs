using Talabat.API.DTO;
using System.Security.Principal;

namespace Talabat.APIs.Helpers
{
    public class Pagenations<T>
    { 
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagenations(int pageIndex, int pageSize, int count, IReadOnlyList<T> data   )
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
            Count = count;
        }
    }
}
