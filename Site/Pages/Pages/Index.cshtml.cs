using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using siteinfo;

namespace site.Pages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        //private readonly IPageRepository pageRepository;

        public SitePage CurrentPage { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int id)
        {
            var page = await _context.SitePages.FirstOrDefaultAsync(p => p.Id == id);
            if (page == null)
                CurrentPage = SitePage.Create("Page Not Found", "The page you are looking for could not be found");
            else
                CurrentPage = SitePage.Create(page.Title, page.Content);
        }

        public sealed class SitePage
        {
            public static SitePage Create(string title, string content)
            {
                var page = new SitePage
                {
                    Title = title,
                    SafeHtmlContent = content
                };

                return page;
            }

            public string Title { get; private set; }
            public string SafeHtmlContent { get; private set; }
        }
    }
}