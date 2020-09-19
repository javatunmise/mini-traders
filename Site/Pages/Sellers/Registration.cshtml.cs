using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Shared;
using site.Data;
using site.Helpers;
using site.Repositories;

namespace site.Pages.Sellers
{
    public class RegistrationModel : PageModel
    {
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";
        private readonly ISiteContentProvider _provider;
        private readonly AccountRepository _accountRepository;
        private readonly IHostEnvironment _environment;
        private readonly StoreRepository _storeRepository;

        public RegistrationModel(ISiteContentProvider provider, 
                                 AccountRepository accountRepository,
                                 StoreRepository storeRepository,
                                 IHostEnvironment env)
        {
            _provider = provider;
            _accountRepository = accountRepository;
            _environment = env;
            _storeRepository = storeRepository;
        }

        public async Task<IActionResult> OnGet(string ref_id = "")
        {
            var currentUser = await _accountRepository.FindByUsername(User.Identity.Name);

            if (currentUser.HasStore)
                return RedirectToPage("/Profile/Store/Index");

            Input = new InputModel { ReferrerCode = ref_id };

            await InitFormData();
            return Page();
        }

        private async Task InitFormData()
        {
            Campuses = (await _provider.GetLocations())
             .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
             .ToList();
        }

        public List<SelectListItem> Campuses { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Means of identification")]
        public IFormFile DocUpload { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Input.Validate();
                var filePath = "";
                if (DocUpload != null)
                {
                    filePath = await CreateFile();
                    if (filePath == UPLOAD_NOT_SUPPORTED)
                    {
                        ModelState.AddModelError(nameof(DocUpload), "Uploaded file format not supported");
                        await InitFormData();
                        return Page();
                    }
                }

                Input.DocumentLocation = filePath;

                var currentUser = await _accountRepository.FindByUsername(User.Identity.Name);
                if (currentUser == null)
                    return RedirectToPage("/Profile/AccountNotFound");

                var storeKeeper = new StoreKeeper();
                var store = storeKeeper.AssignStore(currentUser, Input.CreateRegistrationForm());                
                await _storeRepository.Create(store);

                return RedirectToPage("/Profile/Store/Index");
            }
            else
            {
                await InitFormData();
                return Page();
            }
        }

        private async Task<string> CreateFile()
        {
            if (!StringUtil.TryGetSafeImageExtension(DocUpload.FileName, out string extension))
                return UPLOAD_NOT_SUPPORTED;

            var fileName = $"logo_{StringUtil.SafeGuid()}.{extension}";
            var relativePath = "images/uploads_docs/";
            var path = Path.Combine("wwwroot/" + relativePath, fileName);
            var file = Path.Combine(_environment.ContentRootPath, path);
            using var fileStream = new FileStream(file, FileMode.Create);
            await DocUpload.CopyToAsync(fileStream);
            return relativePath + fileName;
        }
    }

    public class InputModel
    {
        [Required]
        [Display(Name = "Business Name")]
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }

        public string ReferrerCode { get; set; }

        [Required]
        [Display(Name = "Campus")]
        public int CampusId { get; set; }
        public int HostelId { get; set; }
        public string DocumentLocation { get; internal set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone Number")]
        public string ContactPhone {get;set;}
        /// <summary>
        /// Throws invalid argument exception
        /// </summary>
        public void Validate()
        {

        }

        internal SellerRegistrationForm CreateRegistrationForm()
        {
            return new SellerRegistrationForm
            {
                StoreName = StoreName,
                StoreDescription = StoreDescription,
                DocumentLocation = DocumentLocation,
                ReferrerCode = ReferrerCode,
                CampusId = CampusId,
                HostelId = HostelId,
                PhoneNumber = ContactPhone
            };
        }
    }
}