using System;
using System.Collections.Generic;
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

namespace site.Pages.Admin.Banners
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";

        public List<string> BannerPositions = SiteImage.BannerPositions;

        public IndexModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _dbContext = context;
            _environment = env;
        }

        public async Task OnGet()
        {
            Banners = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Banners)
                                .ToListAsync();

            foreach(var used in Banners.Select(e => e.EntityId))
            {
                BannerPositions.Remove(used);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var filePath = await CreateFile(Input.ImageUpload);
            Banners = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Banners)
                   .ToListAsync();

            if (filePath == UPLOAD_NOT_SUPPORTED)
            {
                ModelState.AddModelError(nameof(Input.ImageUpload), "Uploaded file format not supported");
                return Page();
            }

            foreach (var used in Banners.Select(e => e.EntityId))
            {
                BannerPositions.Remove(used);
            }

            if (!BannerPositions.Contains(Input.EntityId))
            {
                ModelState.AddModelError(nameof(Input.EntityId), "Specified position invalid");
                Banners = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Banners)
                      .ToListAsync();
                return Page();
            }

            var carousel = new SiteImage
            {
                Link = Input.Link,
                ImagePath = filePath,
                EntityId = Input.EntityId,
                GroupCode = ImageUploadGroups.Banners,
                Ordering = 1
            };

            _dbContext.SiteImages.Add(carousel);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
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
        public EditModel.FormInput Input { get; set; }
        public List<SiteImage> Banners { get; private set; }
    }
}