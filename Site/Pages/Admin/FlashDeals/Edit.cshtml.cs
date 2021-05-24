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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        [BindProperty]
        public FormInput Input { get; set; }

        public EditModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            var flashDeal = await _dbContext.FlashDeals.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (flashDeal == null)
                return RedirectToPage("/Error404");

            Input = new FormInput
            {
                StartDate = flashDeal.StartDate,
                EndDate = flashDeal.EndDate,
                Name = flashDeal.Name,
                MinDiscount = flashDeal.MinimumDiscount * 100,
                Status = flashDeal.Status
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var flashDeal = await _dbContext.FlashDeals.FirstOrDefaultAsync(x => x.Id == id);
            if (flashDeal == null)
                return RedirectToPage("/Error404");

            var errorMessage = "";
            if (Input.StartDate < DateTime.Now) errorMessage = "Start date must be in the future";
            if (Input.StartDate > Input.EndDate) errorMessage = "Start date must be earlier than End date";
            if (Input.MinDiscount < 1) errorMessage = "Percentage must be between 1 and 99";

            if (!string.IsNullOrEmpty(errorMessage))
            {
                ModelState.AddModelError("", errorMessage);
                return Page();
            }

            if(Input.Status == FlashDealStatuses.Active && flashDeal.Status == FlashDealStatuses.Inactive)
            {
                var active = await _dbContext.FlashDeals.AsNoTracking().FirstOrDefaultAsync(x => x.Status == FlashDealStatuses.Active);
                if(active != null)
                {
                    ModelState.AddModelError("Status", $"Status cannot be updated. Flash deal [{active.Name}] is already active");
                    return Page();
                }
            }

            flashDeal.Name = Input.Name;
            flashDeal.StartDate = Input.StartDate;
            flashDeal.EndDate = Input.EndDate;
            flashDeal.MinimumDiscount = Input.MinDiscount / 100;
            flashDeal.Status = Input.Status;

            await _dbContext.SaveChangesAsync();

            return Redirect("/Admin/Flashdeals");
        }

        public class FormInput
        {
            public string Name { get; set; }
            [Range(1, 100)]
            public decimal MinDiscount { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public FlashDealStatuses Status { get;  set; }
        }
    }
}
