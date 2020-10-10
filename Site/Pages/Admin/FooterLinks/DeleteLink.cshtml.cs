using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Site.Data;

namespace site.Pages.Admin.FooterLinks
{
    public class DeleteLinkModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteLinkModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int footerheader, int link)
        {
            var record = await _context.SiteLinks.FirstOrDefaultAsync(e => e.LinkGroup == (SiteLink.LinkGroups) footerheader && e.Id == link);

            if(record != null)
            {
                _context.SiteLinks.Remove(record);
                await _context.SaveChangesAsync();
            }

            return Redirect($"/Admin/FooterLinks?footerheader={footerheader}");
        }
    }
}