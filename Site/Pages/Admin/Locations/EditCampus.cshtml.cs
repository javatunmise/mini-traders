using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Shared.Entities;

namespace site.Pages.Admin.Locations
{
    public class EditCampusModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public EditCampusModel(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Campus CurrentRecord { get; private set; }

        [BindProperty]
        public IndexModel.CampusDetail Input { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentRecord = await _dbContext.Campuses.FirstOrDefaultAsync(e => e.Id == id);
            if (CurrentRecord == null) return RedirectToPage("/Error404");

            Input = new IndexModel.CampusDetail { Name = CurrentRecord?.Name };

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            CurrentRecord = await _dbContext.Campuses.FirstOrDefaultAsync(e => e.Id == id);
            if (CurrentRecord == null) return RedirectToPage("/Error404");

            if (await NameExist(Input.Name, CurrentRecord.Name, CurrentRecord.Id))
            {
                ModelState.AddModelError("Name", "Name already exist");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Input.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return Page();
            }

            CurrentRecord.Name = Input.Name;
            await _dbContext.SaveChangesAsync();

            return Page();
        }

        private async Task<bool> NameExist(string newName, string oldName, int currentCampusId)
        {
            if (newName.ToLower() == oldName.ToLower()) return false;
            return await _dbContext.Campuses.AnyAsync(e => e.Name == newName && e.Id != currentCampusId);
        }
    }
}