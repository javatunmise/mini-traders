using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using site.Data;
using site.Helpers;
using site.Repositories;

namespace site.Pages.Profile.Store
{
    public class SettingsModel : PageModel
    {
        private readonly ISiteContentProvider _provider;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";
        private readonly AccountRepository _accountRepo;
        private readonly StoreRepository _storeRepo;
        private readonly IHostEnvironment _environment;

        public SettingsModel(ISiteContentProvider provider, 
                             AccountRepository accountRepository, 
                             StoreRepository storeRepository,
                             IHostEnvironment env)
        {
            _accountRepo = accountRepository;
            _storeRepo = storeRepository;
            _environment = env;
            _provider = provider;
        }

        public async Task<IActionResult> OnGet()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var store = currentUser.Store;

            Input = new StoreEdit
            {
                ContactPhone = store.PhoneNumber,
                StoreDescription = store.StoreDescription,
                StoreName = store.Name,
                CampusId = store.CampusId,
                HostelId = store.HostelId
            };

            await InitFormData(store);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            var store = currentUser.Store;

            store.StoreDescription = Input.StoreDescription;
            store.Name = Input.StoreName;
            store.PhoneNumber = Input.ContactPhone;
            store.CampusId = Input.CampusId;
            store.HostelId = Input.HostelId;

            if (VendorImageUpload != null)
            {
                store.LogoPath = await CreateFile();
                if(store.LogoPath == UPLOAD_NOT_SUPPORTED)
                {
                    ModelState.AddModelError(nameof(VendorImageUpload), "Uploaded file format not supported");
                    return Page();
                }
            }

            await _storeRepo.Update(store);

            return Page();
        }

        private async Task<string> CreateFile()
        {
            if (!StringUtil.TryGetSafeImageExtension(VendorImageUpload.FileName, out string extension))
                return UPLOAD_NOT_SUPPORTED;

            var fileName = $"logo_{StringUtil.SafeGuid()}.{extension}";
            var relativePath = "images/uploads_docs/";
            var path = Path.Combine("wwwroot/" + relativePath, fileName);
            var file = Path.Combine(_environment.ContentRootPath, path);
            using var fileStream = new FileStream(file, FileMode.Create);
            await VendorImageUpload.CopyToAsync(fileStream);
            return relativePath + fileName;
        }

        [BindProperty]
        public StoreEdit Input { get; set; }

        [BindProperty]
        public IFormFile VendorImageUpload { get; set; }

        private async Task InitFormData(Shared.Store store)
        {
            Campuses = (await _provider.GetLocations())
             .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
             .ToList();

            Hostels = (await _provider.GetSubLocations(store.CampusId))
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                .ToList();
        }

        public List<SelectListItem> Campuses { get; set; }
        public List<SelectListItem> Hostels { get; set; }
    }

    public class StoreEdit
    {
        [Required]
        [Display(Name = "Campus")]
        public int CampusId { get; set; }

        [Required]
        [Display(Name = "Hostel")]
        public int? HostelId { get; set; }

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