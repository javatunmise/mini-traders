using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Repositories;

namespace site.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly AccountRepository _accountRepository;

        public IndexModel(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            var currentUser = await _accountRepository.FindSiteUser(User.Identity.Name);
            if (currentUser == null)
                return RedirectToPage("AccountNotFound");

            ProfileEdit = new Input();
            ProfileEdit.FirstName = currentUser.FirstName;
            ProfileEdit.LastName = currentUser.LastName;

            return Page();
        }

        public Input ProfileEdit { get; set; }
    }

    public class Input
    {
        [Required][Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}