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
using site.Data;
using site.Data.Repositories;
using site.Helpers;
using Site.Data;

namespace site.Pages.Admin.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CategoriesRepository _categoryRepo;
        private readonly ISiteContentProvider siteContentProvider;
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";

        public EditModel(ApplicationDbContext context,
            CategoriesRepository categoryRepository,
            ISiteContentProvider siteContentProvider,
            IWebHostEnvironment env)
        {
            _dbContext = context;
            _categoryRepo = categoryRepository;
            this.siteContentProvider = siteContentProvider;
            _environment = env;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            var allCategories = await siteContentProvider.GetAllCategories();
            var levelCategories = await _categoryRepo.Get2LevelDeepCategories();
            CurrentCategory = allCategories.FirstOrDefault(e => e.Id == id);
            Input = new FormInput
            {
                CategoryName = CurrentCategory?.Name,
                ParentId = CurrentCategory?.Parent?.Id
            };

            ParentCategories = levelCategories.Where(e => e.Id != id);

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(e => e.Id == id);
            if(category == null) return RedirectToPage("/Error404");

            var filePath = await CreateFile(Input.IconImage);
            if (filePath == UPLOAD_NOT_SUPPORTED)
            {
                ModelState.AddModelError(nameof(Input.IconImage), "Uploaded file format not supported");
                var allCategories = await siteContentProvider.GetAllCategories();
                CurrentCategory = allCategories.FirstOrDefault(e => e.Id == id);
                var levelCategories = await _categoryRepo.Get2LevelDeepCategories();
                ParentCategories = levelCategories.Where(e => e.Id != id);
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(Input.CategoryName))
                category.Name = Input.CategoryName;
            if (!string.IsNullOrWhiteSpace(filePath))
                category.IconImagePath = filePath;

            category.ParentId = Input.ParentId;

            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task<string> CreateFile(IFormFile imageUpload)
        {
            if (imageUpload == null) return "";

            if (!StringUtil.TryGetSafeImageExtension(imageUpload.FileName, out string extension))
                return UPLOAD_NOT_SUPPORTED;

            var fileName = $"categories_{StringUtil.SafeGuid()}.{extension}";
            var relativePath = "images/adr_images/";
            var path = Path.Combine("wwwroot/" + relativePath, fileName);
            var file = Path.Combine(_environment.ContentRootPath, path);
            using var fileStream = new FileStream(file, FileMode.Create);
            await imageUpload.CopyToAsync(fileStream);
            return relativePath + fileName;
        }

        [BindProperty]
        public FormInput Input { get; set; }
        public IEnumerable<Shared.Entities.Category> ParentCategories { get; private set; }
        public Category CurrentCategory { get; private set; }


        public class FormInput
        {
            [Required]
            [Display(Name = "Category")]
            public string CategoryName { get; set; }
            public IFormFile IconImage { get; set; }
            public int? ParentId { get; set; }
        }
    }
}