using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Site.Data;

namespace site.Pages.Admin.FlashDeals
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public List<FlashDeal> FlashDeals { get; private set; }
        [BindProperty]
        public FormInput Input { get; set; }

        public IndexModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task OnGet()
        {
            FlashDeals = await _dbContext.FlashDeals.AsNoTracking().OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var errorMessage = "";
            if (Input.StartDate < DateTime.Now) errorMessage = "Start date must be in the future";
            if (Input.StartDate > Input.EndDate) errorMessage = "Start date must be earlier than End date";
            if (Input.MinDiscount < 1) errorMessage = "Percentage must be between 1 and 99";

            FlashDeals = await _dbContext.FlashDeals.AsNoTracking().OrderByDescending(x => x.Id).ToListAsync();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError("", errorMessage);
                return Page();
            }

            var deal = new FlashDeal
            {
                StartDate = Input.StartDate,
                EndDate = Input.EndDate,
                MinimumDiscount = Input.MinDiscount / 100,
                Name = Input.Name
            };

            _dbContext.FlashDeals.Add(deal);

            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }

        public class FormInput
        {
            [Required]
            public string Name { get; set; }
            [Range(1,100)]
            public decimal MinDiscount { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}
