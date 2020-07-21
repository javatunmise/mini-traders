using System;
using System.Collections.Generic;
using System.Text;

namespace siteinfo
{
    public class Page
    {
        public int Id { get; set; }
        public string Slug { get; internal set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class PageRepository : IPageRepository
    {
        public IEnumerable<Page> GetPages()
        {
            return new List<Page>
            {
                new Page { Slug = "payment-policy", Title = "Payment Policy" },
                new Page { Slug = "return-policy", Title = "Return Policy" },
                new Page { Slug = "privacy-policy", Title = "Privacy Policy" },
                new Page { Slug = "faq", Title = "FAQ" },
                new Page { Slug = "terms-and-condition", Title = "Terms and Condition" },
                new Page { Slug = "delivery-info", Title = "Delivery Information" },
            };
        }
    }
}
