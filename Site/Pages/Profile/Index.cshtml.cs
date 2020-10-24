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
using site.Helpers;
using site.Repositories;

namespace site.Pages.Profile
{
    public class IndexModel : PageModel
    {
        private readonly AccountRepository _accountRepository;
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";
        private readonly UserManager<IdentityUser>  _userManager;

        public IndexModel(AccountRepository accountRepository,
                                 UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _accountRepository = accountRepository;
            _environment = env;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            var currentUser = await _accountRepository.FindSiteUser(User.Identity.Name);
            if (currentUser == null)
                return RedirectToPage("AccountNotFound");

            var identityUser = await _userManager.GetUserAsync(User);
            if (!await _userManager.IsEmailConfirmedAsync(identityUser))
                return RedirectToPage("/EmailNotConfirmed");

            ProfileEdit = new Input
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _accountRepository.FindSiteUser(User.Identity.Name);
            currentUser.FirstName = ProfileEdit.FirstName;
            currentUser.LastName = ProfileEdit.LastName;

            if (ImageUpload != null)
            {
                currentUser.ProfilePicturePath = await CreateFile();
                if (currentUser.ProfilePicturePath == UPLOAD_NOT_SUPPORTED)
                {
                    ModelState.AddModelError(nameof(ImageUpload), "Uploaded file format not supported");
                    return Page();
                }
            }

            await _accountRepository.UpdateSiteUser(currentUser);

            return Page();
        }

        private async Task<string> CreateFile()
        {
            if (!StringUtil.TryGetSafeImageExtension(ImageUpload.FileName, out string extension))
                return UPLOAD_NOT_SUPPORTED;

            var fileName = $"profile_pic_{StringUtil.SafeGuid()}.{extension}";
            var relativePath = "images/uploads_docs/";
            var path = Path.Combine("wwwroot/" + relativePath, fileName);
            var file = Path.Combine(_environment.ContentRootPath, path);
            using var fileStream = new FileStream(file, FileMode.Create);
            await ImageUpload.CopyToAsync(fileStream);
            return relativePath + fileName;
        }

        [BindProperty]
        public Input ProfileEdit { get; set; }

        [BindProperty]
        public IFormFile ImageUpload { get; set; }
    }

    public class Input
    {
        [Required][Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        public string Email { get; internal set; }
    }
}