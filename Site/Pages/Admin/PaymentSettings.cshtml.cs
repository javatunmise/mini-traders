using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using site.Data;
using Site.Data;

namespace site.Pages.Admin
{
    public class PaymentSettingsModel : PageModel
    {
        public ApplicationDbContext _dbContext { get; }

        [BindProperty]
        public FormInput Input { get; set; }
        public bool Success { get; private set; }

        private readonly ISiteContentProvider _provider;

        public PaymentSettingsModel(ApplicationDbContext context, ISiteContentProvider siteContentProvider)
        {
            _dbContext = context;
            _provider = siteContentProvider;
        }

        public async Task OnGet()
        {
            var site = await _provider.GetCurrentSite();

            Input = new FormInput
            {
                WithdrawalCharge = site.WithdrawalCharge,
                SignonFee = site.SignOnFee
            };
        }
        public async Task<IActionResult> OnPostAsync()
        {
            ValidateForm();

            if (ModelState.IsValid)
            {
                var site = await _provider.GetCurrentSite();

                site.WithdrawalCharge = Input.WithdrawalCharge;
                site.SignOnFee = Input.SignonFee;

                await _dbContext.SaveChangesAsync();

                Success = true;
            }

            return Page();
        }

        private void ValidateForm()
        {
            if (Input.WithdrawalCharge < 0)
                ModelState.AddModelError(nameof(FormInput.WithdrawalCharge), "Must not be less than 0");
            if (Input.SignonFee < 0)
                ModelState.AddModelError(nameof(FormInput.SignonFee), "Must not be less than 0");
        }

        public class FormInput
        {
            [Required]
            [DataType(DataType.Currency)]
            public decimal WithdrawalCharge { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public decimal SignonFee { get; set; }
        }
    }
}
