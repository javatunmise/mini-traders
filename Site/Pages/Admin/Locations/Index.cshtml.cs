using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace site.Pages.Admin.Locations
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public List<Campus> Campuses { get; private set; }

        public async Task OnGet()
        {
            Campuses = await _dbContext.Campuses.ToListAsync();
        }

        public async Task<IActionResult> OnPost()
        {
            if (await NameExist(Input.Name))
            {
                ModelState.AddModelError("Name", "Name already exist");
                Campuses = await _dbContext.Campuses.ToListAsync();
                return Page();
            }

            _dbContext.Campuses.Add(new Campus { Name = Input.Name });

            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task<bool> NameExist(string name)
        {
            return await _dbContext.Campuses.AnyAsync(e => e.Name == name);
        }

        [BindProperty]
        public CampusDetail Input { get; set; }

        public class CampusDetail
        {
            [Required]
            public string Name { get; set; }
        }
    }

}