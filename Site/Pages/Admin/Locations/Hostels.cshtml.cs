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
    public class HostelsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public HostelsModel(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Campus CurrentRecord { get; private set; }
        public List<Hostel> Hostels { get; private set; }

        [BindProperty]
        public IndexModel.CampusDetail Input { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentRecord = await _dbContext.Campuses.FirstOrDefaultAsync(e => e.Id == id);
            if (CurrentRecord == null) return RedirectToPage("/Error404");

            Hostels = await _dbContext.Hostels.Where(e => e.CampusId == id).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            CurrentRecord = await _dbContext.Campuses.FirstOrDefaultAsync(e => e.Id == id);
            if (CurrentRecord == null) return RedirectToPage("/Error404");

            if (string.IsNullOrWhiteSpace(Input.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                Hostels = await _dbContext.Hostels.Where(e => e.CampusId == id).ToListAsync();
                return Page();
            }

            if (await NameExist(Input.Name, CurrentRecord))
            {
                ModelState.AddModelError("Name", "Name already exist");
                Hostels = await _dbContext.Hostels.Where(e => e.CampusId == id).ToListAsync();
                return Page();
            }

            _dbContext.Hostels.Add(new Hostel { Name = Input.Name, CampusId = id });

            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task<bool> NameExist(string name, Campus campus)
        {
            return await _dbContext.Hostels.AnyAsync(e => e.CampusId == campus.Id && e.Name == name);
        }
    }
}