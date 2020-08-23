using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Repositories;

namespace site.Pages.Profile.Store
{
    public class IndexModel : PageModel
    {
        private readonly AccountRepository _accountRepository;

        public async Task OnGet()
        {
            var currentUser = await _accountRepository.FindByUsername(User.Identity.Name);
            var store = currentUser.Store;
        }
    }
}