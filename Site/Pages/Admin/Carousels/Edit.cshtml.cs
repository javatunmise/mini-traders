using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using site.Helpers;
using Site.Data;

namespace site.Pages.Admin.Carousels
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";

        public EditModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _dbContext = context;
            _environment = env;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var carousels = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Carousels)
                                .ToListAsync();

            var carousel = carousels.FirstOrDefault(e => int.Parse(e.EntityId) == id);
            if (carousel == null) 
               return RedirectToPage("/Error404");

            Input = new FormInput
            {
                Link = carousel.Link
            };

            ImagePath = carousel.ImagePath;

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var handler = Request.Form["handler"];
            if (handler == "ondelete") return await OnDelete(id);

            var carousels = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Carousels)
                                .ToListAsync();

            var carousel = carousels.FirstOrDefault(e => int.Parse(e.EntityId) == id);
            if (carousel == null) return Page();

            var filePath = await CreateFile(Input.ImageUpload);
            if (filePath == UPLOAD_NOT_SUPPORTED)
            {
                ModelState.AddModelError(nameof(Input.ImageUpload), "Uploaded file format not supported");
                return Page();
            }
            
            carousel.Link = Input.Link;
            if (!string.IsNullOrWhiteSpace(filePath))
                carousel.ImagePath = filePath;

            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnDelete(int id)
        {
            var carousels = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Carousels)
                                .ToListAsync();

            var carousel = carousels.FirstOrDefault(e => int.Parse(e.EntityId) == id);
            if (carousel == null) return Page();

            _dbContext.SiteImages.Remove(carousel);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Admin/Carousels/Index");
        }

        private async Task<string> CreateFile(IFormFile imageUpload)
        {
            if (imageUpload == null) return "";

            if (!StringUtil.TryGetSafeImageExtension(imageUpload.FileName, out string extension))
                return UPLOAD_NOT_SUPPORTED;

            var fileName = $"profile_pic_{StringUtil.SafeGuid()}.{extension}";
            var relativePath = "images/adr_images/";
            var path = Path.Combine("wwwroot/" + relativePath, fileName);
            var file = Path.Combine(_environment.ContentRootPath, path);
            using var fileStream = new FileStream(file, FileMode.Create);
            await imageUpload.CopyToAsync(fileStream);
            return relativePath + fileName;
        }

        [BindProperty]
        public FormInput Input { get; set; }
        public string ImagePath{ get; private set; }

        public class FormInput
        {
            [Required]
            public string Link { get; set; }

            [Required]
            [Display(Name = "Image")]
            public IFormFile ImageUpload { get; set; }
            public string EntityId { get; set; }
        }
    }


}