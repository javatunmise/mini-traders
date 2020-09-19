using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Repositories;

namespace site.Pages.Profile
{
    public class BalancesModel : PageModel
    {
        private readonly AccountRepository _accountRepo;

        public BalancesModel(AccountRepository accountRepository)
        {
            _accountRepo = accountRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            return Page();
        }
    }
}