using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using siteinfo;

namespace site.Pages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPageRepository pageRepository;

        public SitePage CurrentPage { get; set; }

        public IndexModel(IPageRepository pageRepository)
        {
            this.pageRepository = pageRepository;
        }

        public void OnGet(string id)
        {
            var page = pageRepository.GetPages().FirstOrDefault(p => p.Slug == id);
            if (page == null)
                CurrentPage = SitePage.Create("Page Not Found", "The page you are looking for could not be found");
            else
                CurrentPage = SitePage.Create(page.Title, page.Content ?? "<p style='color: magenta'>Some raw content</p>");
        }

        public sealed class SitePage
        {
            public static SitePage Create(string title, string content)
            {
                var page = new SitePage();
                page.Title = title;
                page.SafeHtmlContent = content;
                return page;
            }

            public string Title { get; private set; }
            public string SafeHtmlContent { get; private set; }
        }
    }
}