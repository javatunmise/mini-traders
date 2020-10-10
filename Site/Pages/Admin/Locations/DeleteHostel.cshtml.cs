using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using site.Data;
using site.Data.Repositories;
using Site.Data;
using Shared.Entities;

namespace site.Pages.Admin.Categories
{
    public class DeleteHostelModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteHostelModel(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentRecord = await _dbContext.Hostels.FirstOrDefaultAsync(e => e.Id == id);
            if (CurrentRecord == null) return RedirectToPage("/Error404");

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            CurrentRecord = await _dbContext.Hostels.FirstOrDefaultAsync(e => e.Id == id);
            if (CurrentRecord == null) return RedirectToPage("/Error404");

            _dbContext.Hostels.Remove(CurrentRecord);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage($"/Admin/Locations/Hostels", new { id = CurrentRecord?.CampusId } );
        }

        public Hostel CurrentRecord { get; private set; }
    }
}