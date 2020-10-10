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
    public class DeleteCampusModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteCampusModel(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            HasHostels = await _dbContext.Hostels.AnyAsync(e => e.CampusId == id);
            CurrentRecord = await _dbContext.Campuses.FirstOrDefaultAsync(e => e.Id == id);

            if (CurrentRecord == null) return RedirectToPage("/Error404");

            if (HasHostels)
            {
                CanDelete = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            HasHostels = await _dbContext.Hostels.AnyAsync(e => e.CampusId == id);
            CurrentRecord = await _dbContext.Campuses.FirstOrDefaultAsync(e => e.Id == id);

            if (CurrentRecord == null) return RedirectToPage("/Error404");

            if (HasHostels)
            {
                CanDelete = false;
                return Page();
            }

            _dbContext.Campuses.Remove(CurrentRecord);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Admin/Locations/Index");
        }

        public bool CanDelete { get; private set; }
        public bool HasHostels { get; private set; }
        public Campus CurrentRecord { get; private set; }
    }
}