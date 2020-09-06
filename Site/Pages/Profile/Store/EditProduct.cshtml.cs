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
using Microsoft.Extensions.Logging;
using NuGet.Frameworks;
using site.Data;
using site.Helpers;
using site.Repositories;

namespace site.Pages.Profile.Store
{
    public class EditProductModel : PageModel
    {
        private readonly AccountRepository _accountRepo;
        private readonly ISiteContentProvider _siteContentProvider;
        private readonly IWebHostEnvironment _environment;
        private readonly ProductsRepository _productRepository;
        private readonly ILogger<CreateProductModel> _logger;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";

        public EditProductModel(AccountRepository accountRepository, 
                                  ISiteContentProvider siteContentProvider,
                                  ProductsRepository productsRepository,
                                  IWebHostEnvironment env,
                                  ILogger<CreateProductModel> logger)
        {
            _accountRepo = accountRepository;
            _siteContentProvider = siteContentProvider;
            _environment = env;
            _productRepository = productsRepository;
            _logger = logger;

            FormInput = new Input();
        }

        public IEnumerable<Category> TopCategories { get; private set; }

        [BindProperty]
        public Input FormInput { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var product = await _productRepository.GetProduct(id);

            FormInput = new Input
            {
                Name = product.Name,
                Description = product.ProductDetails,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Specifications = product.Specifications
            };

            await LoadDropdownCategories();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var productImagePath = "";
            if (FormInput.ImageUpload != null)
            {
                productImagePath = await CreateFile();
                if (productImagePath == UPLOAD_NOT_SUPPORTED)
                {
                    ModelState.AddModelError(nameof(FormInput.ImageUpload), "Uploaded file format not supported");
                    return Page();
                }
            }

            var product = new Shared.Entities.Product
            {
                Id = id,
                Name = FormInput.Name,
                ProductDetails = FormInput.Description,
                Price = FormInput.Price,
                OldPrice = FormInput.Price,
                CategoryId = FormInput.CategoryId,
                ImageUrl = productImagePath,
                SmallImageUrl = productImagePath,
                StoreId = currentUser.Store.Id,
                Specifications = FormInput.Specifications
            };

            try
            {
                await _productRepository.Update(product);
                return RedirectToPage("/Profile/Store/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unable to save the record: {ex.Message}. Please try again");
                _logger.LogDebug(ex.Message);
            }

            await LoadDropdownCategories();

            return Page();
        }

        private async Task<string> CreateFile()
        {
            if (!StringUtil.TryGetSafeImageExtension(FormInput.ImageUpload.FileName, out string extension))
                return UPLOAD_NOT_SUPPORTED;

            var fileName = $"logo_{StringUtil.SafeGuid()}.{extension}";
            var relativePath = "images/products/";
            var path = Path.Combine("wwwroot/" + relativePath, fileName);
            var file = Path.Combine(_environment.ContentRootPath, path);
            using var fileStream = new FileStream(file, FileMode.Create);
            await FormInput.ImageUpload.CopyToAsync(fileStream);
            return relativePath + fileName;
        }

        private async Task LoadDropdownCategories()
        {
            var categories = await _siteContentProvider.GetAllCategories();
            foreach (var c in categories)
            {
                c.Children = categories.Where(ch => ch.Parent?.Id == c.Id);
            }

            foreach (var c in categories)
            {
                if (Is3rdLevelDeep(c, FindParent(c, categories)))
                    c.Name = $"-------- {c.Name}";
                else if (Is2ndLevelDeep(c))
                    c.Name = $"---- {c.Name}";
            }

            TopCategories = categories;
        }

        private Category FindParent(Category c, IList<Category> categories) => categories.FirstOrDefault(e => e.Id == c.Parent?.Id);

        private bool Is3rdLevelDeep(Category c, Category parent) => c.Parent != null && parent.Parent != null;
        private bool Is2ndLevelDeep(Category c) => c.Parent != null;

        public class Input
        {
            [Required]
            [Display(Name = "Category")]
            public int CategoryId { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            [Display(Name = "Description")]
            public string Description { get; set; }

            public string Specifications { get; set; }

            [Display(Name = "Price")]
            [DataType(DataType.Currency)]
            public decimal Price { get; set; }

            [Display(Name = "Image")]
            public IFormFile ImageUpload { get; set; }
        }
    }
}