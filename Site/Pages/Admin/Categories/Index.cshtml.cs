using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Dapper;
using site.Data.Repositories;
using Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using site.Helpers;
using System.ComponentModel.DataAnnotations;

namespace site.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CategoriesRepository _categoryRepo;
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";

        public IndexModel(ApplicationDbContext context, CategoriesRepository categoryRepository, IWebHostEnvironment env)
        {
            _dbContext = context;
            _categoryRepo = categoryRepository;
            _environment = env;
        }

        public List<CategoryView> Categories { get; private set; }
        public Category ParentCategory { get; set; }

        [BindProperty]
        public FormInput Input { get; set; }

        public async Task OnGet()
        {
            Categories = (await _categoryRepo.GetCategorySummary(null)).ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            var filePath = await CreateFile(Input.IconImage);
            if (filePath == UPLOAD_NOT_SUPPORTED)
            {
                ModelState.AddModelError(nameof(Input.IconImage), "Uploaded file format not supported");
                return Page();
            }

            var category = new Category
            {
                Name = Input.CategoryName,
                IconImagePath = filePath
            };

            _dbContext.Categories.Add(category);
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

        public class FormInput
        {
            [Required]
            [Display(Name = "Category")]
            public string CategoryName { get; set; }
            [Required]          
            public IFormFile IconImage { get; set; }
            public int? ParentId { get; set; }
        }
    }
}
