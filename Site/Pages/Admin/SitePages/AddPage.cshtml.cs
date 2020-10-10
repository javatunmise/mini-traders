using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared;
using Shared.Entities;
using Site.Data;

namespace site.Pages.Admin.SitePages
{
    public class AddPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentDate _currentDate;

        public AddPageModel(ApplicationDbContext context, ICurrentDate currentDate)
        {
            _context = context;
            _currentDate = currentDate;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (Input.FileUpload == null)
            {
                ModelState.AddModelError(nameof(Input.FileUpload), "File is required");
                return Page();
            }

            if (!TryGetSafeImageExtension(Input.FileUpload.FileName, out string extension))
            {
                ModelState.AddModelError(nameof(Input.FileUpload), "Only .txt file is supported");
                return Page();
            }

            _context.SitePages.Add(new SitePage
            {
                Title = Input.Title,
                Content = await ReadFileContent(Input.FileUpload),
                CreatedOn = _currentDate.Now(),
                LastModifiedOn = _currentDate.Now()
            });

            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/SitePages/Index");
        }

        private async Task<string> ReadFileContent(IFormFile fileUpload)
        {
            using var stream = new StreamReader(fileUpload.OpenReadStream());
            return await stream.ReadToEndAsync();
        }

        internal static bool TryGetSafeImageExtension(string fileName, out string extension)
        {
            var lastDot = fileName.LastIndexOf('.');
            extension = lastDot > 0 ? fileName.Substring(lastDot + 1).ToLower() : "";

            return extension == "txt";
        }

            [BindProperty]
        public FormInput Input { get; set; }

        public class FormInput
        {
            [Required]
            public string Title { get; set; }

            [Required]
            [Display(Name = "File")]
            public IFormFile FileUpload { get; set; }
        }
    }
}