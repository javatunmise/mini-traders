using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Repositories;

namespace site.Pages.Profile.Store
{
    public class SettingsModel : PageModel
    {
        private readonly AccountRepository _accountRepo;
        private readonly StoreRepository _storeRepo;

        public SettingsModel(AccountRepository accountRepository, StoreRepository storeRepository)
        {
            _accountRepo = accountRepository;
            _storeRepo = storeRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile");

            var store = currentUser.Store;

            Input = new StoreEdit
            {
                ContactPhone = store.PhoneNumber,
                StoreDescription = store.StoreDescription,
                StoreName = store.Name
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            var store = currentUser.Store;

            store.StoreDescription = Input.StoreDescription;
            store.Name = Input.StoreName;
            store.PhoneNumber = Input.ContactPhone;

            await _storeRepo.Update(store);

            return Page();
        }

        [BindProperty]
        public StoreEdit Input { get; set; }
    }

    public class StoreEdit
    {
        [Required]
        [Display(Name = "Business Name")]
        public string StoreName { get; set; }

        public string StoreDescription { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone Number")]
        public string ContactPhone { get; set; }

    }
}