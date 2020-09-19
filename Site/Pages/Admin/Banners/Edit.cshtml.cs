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
using Microsoft.Extensions.Logging;
using Shared.Entities;
using site.Helpers;
using Site.Data;

namespace site.Pages.Admin.Banners
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<EditModel> _logger;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";

        public EditModel(ApplicationDbContext context, 
                        IWebHostEnvironment env,
                        ILogger<EditModel> logger)
        {
            _dbContext = context;
            _environment = env;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            var banners = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Banners)
                                .ToListAsync();

            var banner = banners.FirstOrDefault(e => e.EntityId == id);
            if (banner == null) 
               return RedirectToPage("/Error404");

            Input = new FormInput
            {
                Link = banner.Link,
                EntityId = banner.EntityId
            };

            ImagePath = banner.ImagePath;

            return Page();
        }

        public async Task<IActionResult> OnPost(string id)
        {
            var handler = Request.Form["handler"];
            if (handler == "ondelete") return await OnDelete(id);

            var banners = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Banners)
                                .ToListAsync();

            var banner = banners.FirstOrDefault(e => e.EntityId == id);
            if (banner == null) return Page();

            var filePath = await CreateFile(Input.ImageUpload);
            if (filePath == UPLOAD_NOT_SUPPORTED)
            {
                ModelState.AddModelError(nameof(Input.ImageUpload), "Uploaded file format not supported");
                return Page();
            }
            
            banner.Link = Input.Link;

            if (!string.IsNullOrWhiteSpace(filePath))
                banner.ImagePath = filePath;

            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }


        public async Task<IActionResult> OnDelete(string id)
        {
            var banners = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Banners)
                                .ToListAsync();


            var banner = banners.FirstOrDefault(e => e.EntityId == id);
            if (banner == null) return Page();

            _dbContext.SiteImages.Remove(banner);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("/Admin/Banners/Index");
        }

            private async Task<string> CreateFile(IFormFile imageUpload)
        {
            if (imageUpload == null) return "";

            if (!StringUtil.TryGetSafeImageExtension(imageUpload.FileName, out string extension))
                return UPLOAD_NOT_SUPPORTED;

            var fileName = $"banners_{StringUtil.SafeGuid()}.{extension}";
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

            [Required]
            [Display(Name = "Position")]
            public string EntityId { get; set; }
        }
    }


}