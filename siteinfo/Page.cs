using System;
using System.Collections.Generic;
using System.Text;

namespace siteinfo
{
    public class Page
    {
        public string Slug { get; internal set; }
    }

    public class PageRepository
    {
        public IEnumerable<Page> GetPages()
        {
            return new List<Page>
            {
                new Page { Slug = "payment-policy"},
                new Page { Slug = "return-policy"},
                new Page { Slug = "privacy-policy"},
                new Page { Slug = "faq"},
                new Page { Slug = "terms-and-condition"},
                new Page { Slug = "delivery-info"},
            };
        }
    }
}
