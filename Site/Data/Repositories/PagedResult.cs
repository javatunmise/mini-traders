using System.Collections.Generic;

namespace site.Repositories
{
    public class PagedResult<T>
    {
        public int TotalRecords { get; set; }
        public List<T> Records { get; set; }
    }
}