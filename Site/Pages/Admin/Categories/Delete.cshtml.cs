using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Data;
using site.Data.Repositories;
using Site.Data;

namespace site.Pages.Admin.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CategoriesRepository _categoryRepo;
        private readonly ISiteContentProvider siteContentProvider;

        public DeleteModel(ApplicationDbContext context,
            CategoriesRepository categoryRepository,
            ISiteContentProvider siteContentProvider)
        {
            _dbContext = context;
            _categoryRepo = categoryRepository;
            this.siteContentProvider = siteContentProvider;
            Dependencies = new List<Shared.AggregateDto>();
        }

        public async Task<IActionResult> OnGet(int id)
        {
            IEnumerable<Shared.AggregateDto> dependencies = await _categoryRepo.GetDependentObjects(id);
            CurrentCategory = (await siteContentProvider.GetAllCategories()).FirstOrDefault(e => e.Id == id);
           if (dependencies.Any())
            {
                Dependencies = dependencies.ToList();
                CanDelete = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            IEnumerable<Shared.AggregateDto> dependencies = await _categoryRepo.GetDependentObjects(id);
            CurrentCategory = (await siteContentProvider.GetAllCategories()).FirstOrDefault(e => e.Id == id);
            if (dependencies.Any())
            {
                CanDelete = false;
                Dependencies = dependencies.ToList();
                return Page();
            }

            await _categoryRepo.Delete(id);

            return RedirectToPage("/Admin/Categories/Index");
        }

        public bool CanDelete { get; private set; }
        public List<Shared.AggregateDto> Dependencies { get; private set; }
        public Category CurrentCategory { get; private set; }
    }
}