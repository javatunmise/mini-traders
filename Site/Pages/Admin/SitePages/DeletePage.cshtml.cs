using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Site.Data;

namespace site.Pages.Admin.SitePages
{
    public class DeletePageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeletePageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var record = await _context.SitePages
                                .FirstOrDefaultAsync(e => e.Id == id);

            if(record != null)
            {
                _context.SitePages.Remove(record);
                await _context.SaveChangesAsync();
            }

            return Redirect($"/Admin/SitePages");
        }
    }
}