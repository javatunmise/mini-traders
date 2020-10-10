using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Shared.Entities;

namespace site.Pages.Admin.SitePages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SitePage> Pages { get; private set; }

        public async Task OnGet()
        {
            Pages = await _context.SitePages.AsNoTracking().Select(e => new SitePage
            {
                Id = e.Id,
                Title = e.Title,
                Content = "",
                CreatedOn = e.CreatedOn,
                LastModifiedOn = e.LastModifiedOn
            }).ToListAsync();
        }
    }
}