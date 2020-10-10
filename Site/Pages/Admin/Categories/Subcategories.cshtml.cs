using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Entities;
using site.Data;
using site.Data.Repositories;
using site.Helpers;
using Site.Data;

namespace site.Pages.Admin.Categories
{
    public class SubcategoriesModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CategoriesRepository _categoryRepo;
        private readonly IWebHostEnvironment _environment;
        private readonly ISiteContentProvider siteContentProvider;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";

        public SubcategoriesModel(ApplicationDbContext context, 
            CategoriesRepository categoryRepository, 
            IWebHostEnvironment env,
            ISiteContentProvider siteContentProvider)
        {
            _dbContext = context;
            _categoryRepo = categoryRepository;
            _environment = env;
            this.siteContentProvider = siteContentProvider;
        }

        public List<CategoryView> Categories { get; private set; }
        public site.Data.Category CurrentCategory { get; private set; }
        public bool CanAddSubcategory { get; set; }

        [BindProperty]
        public IndexModel.FormInput Input { get; set; }

        public async Task OnGet(int id)
        {
            Categories = (await _categoryRepo.GetCategorySummary(id)).ToList();

            var allCategories = await siteContentProvider.GetAllCategories();
            CurrentCategory = allCategories.First(e => e.Id == id);

            CanAddSubcategory = !Is3rdLevelDeep(CurrentCategory, CurrentCategory.Parent);
        }

        public async Task<IActionResult> OnPost(int id)
        {
            Categories = (await _categoryRepo.GetCategorySummary(id)).ToList();
            var allCategories = await siteContentProvider.GetAllCategories();
            CurrentCategory = allCategories.First(e => e.Id == id);
            if (!Is3rdLevelDeep(CurrentCategory, CurrentCategory.Parent))
            {
                ModelState.AddModelError("", "More subcategories not allowed");
                return Page();
            }

            var filePath = await CreateFile(Input.IconImage);
            if (filePath == UPLOAD_NOT_SUPPORTED)
            {
                ModelState.AddModelError(nameof(Input.IconImage), "Uploaded file format not supported");
                return Page();
            }

            var category = new Shared.Entities.Category
            {
                Name = Input.CategoryName,
                IconImagePath = filePath,
                ParentId = id
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }

        private bool Is3rdLevelDeep(Data.Category c, Data.Category parent) => c.Parent != null && parent.Parent != null;

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
    }
}