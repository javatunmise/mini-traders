using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Shared.Entities;
using System.IO;

namespace site.Pages.Admin.SitePages
{
    public class EditPageModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditPageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentRecord = await _context.SitePages.FirstOrDefaultAsync(e => e.Id == id);
            if (CurrentRecord == null) return RedirectToPage("/Error404");


            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            CurrentRecord = await _context.SitePages.FirstOrDefaultAsync(e => e.Id == id);
            if (CurrentRecord == null) return RedirectToPage("/Error404");

            if (!string.IsNullOrWhiteSpace(Input.Title))
                CurrentRecord.Title = Input.Title;

            if(Input.FileUpload != null)
            {
                if (!TryGetSafeImageExtension(Input.FileUpload.FileName, out string extension))
                {
                    ModelState.AddModelError(nameof(Input.FileUpload), "Only .txt file is supported");
                    return Page();
                }

                CurrentRecord.Content = await ReadFileContent(Input.FileUpload);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        internal static bool TryGetSafeImageExtension(string fileName, out string extension)
        {
            var lastDot = fileName.LastIndexOf('.');
            extension = lastDot > 0 ? fileName.Substring(lastDot + 1).ToLower() : "";

            return extension == "txt";
        }

        private async Task<string> ReadFileContent(IFormFile fileUpload)
        {
            using var stream = new StreamReader(fileUpload.OpenReadStream());
            return await stream.ReadToEndAsync();
        }

        [BindProperty]
        public FormInput Input { get; set; }
        public SitePage CurrentRecord { get; private set; }

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