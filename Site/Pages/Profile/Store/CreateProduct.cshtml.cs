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
using Newtonsoft.Json;

namespace site.Pages.Profile.Store
{
    public class CreateProductModel : PageModel
    {
        private readonly AccountRepository _accountRepo;
        private readonly ISiteContentProvider _siteContentProvider;
        private readonly IWebHostEnvironment _environment;
        private readonly ProductsRepository _productRepository;
        private readonly StoreRepository _storeRepo;
        private readonly ILogger<CreateProductModel> _logger;
        private const string UPLOAD_NOT_SUPPORTED = "NOT_SUPPORTED";

        public CreateProductModel(AccountRepository accountRepository, 
                                  ISiteContentProvider siteContentProvider,
                                  ProductsRepository productsRepository,
                                  StoreRepository storeRepository,
                                  IWebHostEnvironment env,
                                  ILogger<CreateProductModel> logger)
        {
            _accountRepo = accountRepository;
            _siteContentProvider = siteContentProvider;
            _environment = env;
            _productRepository = productsRepository;
            _storeRepo = storeRepository;
            _logger = logger;

            FormInput = new Input();
        }

        public IEnumerable<Category> TopCategories { get; private set; }

        [BindProperty]
        public Input FormInput { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            await LoadDropdownCategories();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            IList<string> productImagePaths = new[]{""};
            if (FormInput.ImageUpload != null && FormInput.ImageUpload.Any())
            {            
                if (FormInput.ImageUpload.Count > 5)
                {
                    ModelState.AddModelError(nameof(FormInput.ImageUpload), "Only 5 pictures allowed");
                    await LoadDropdownCategories();
                    return Page();
                }

                productImagePaths = await CreateFile();
                if (productImagePaths[0] == UPLOAD_NOT_SUPPORTED)
                {
                    ModelState.AddModelError(nameof(FormInput.ImageUpload), "Uploaded file format not supported");
                    await LoadDropdownCategories();
                    return Page();
                }
            }

            var categories = await _siteContentProvider.GetAllCategories();

            var product = new Shared.Entities.Product
            {
                Name = FormInput.Name,
                ProductDetails = FormInput.Description,
                Price = FormInput.Price,
                OldPrice = FormInput.Price,
                CategoryId = FormInput.CategoryId,
                ImageUrl = productImagePaths[0],
                SmallImageUrl = productImagePaths[0],
                StoreId = currentUser.Store.Id,
                RenderedAsService = StoreUtil.IsServiceCategory(FormInput.CategoryId, categories.ToList()),
                OtherImageUrlsJson = JsonConvert.SerializeObject(productImagePaths)
            };

            try
            {
                await _productRepository.Create(product);
                var filter = product.RenderedAsService ? "services" : "";
                return Redirect($"/Profile/Store?filter={filter}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unable to save the record: {ex.Message}. Please try again");
                _logger.LogDebug(ex.Message);
            }

            await LoadDropdownCategories();

            return Page();
        }

        private async Task<IList<string>> CreateFile()
        {
            var filePaths = await Task.Run<IList<string>>(async () =>
            {
                var filePaths = new List<string>();
                foreach (var upload in FormInput.ImageUpload)
                {
                    if (!StringUtil.TryGetSafeImageExtension(upload.FileName, out string extension))
                        return new[] { UPLOAD_NOT_SUPPORTED };

                    var fileName = $"item_{StringUtil.SafeGuid()}.{extension}";
                    var relativePath = "images/products/";
                    var path = Path.Combine("wwwroot/" + relativePath, fileName);
                    var file = Path.Combine(_environment.ContentRootPath, path);
                    using var fileStream = new FileStream(file, FileMode.Create);
                    await upload.CopyToAsync(fileStream);

                    filePaths.Add(relativePath + fileName);
                }

                return filePaths;
            });

            return filePaths;
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
            [Display(Name = "Price")]
            [DataType(DataType.Currency)]
            public int Price { get; set; }
            [Required]
            [Display(Name = "Image")]
            public List<IFormFile> ImageUpload { get; set; }

            public string Specifications { get; set; }
        }
    }
}