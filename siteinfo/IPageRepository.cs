using System.Collections.Generic;

namespace siteinfo
{
    public interface IPageRepository
    {
        IEnumerable<Page> GetPages();
    }
}