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
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared;
//using Shared.Entities;
using System.Xml.XPath;

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
        public SelectList StatusList { get; private set; }
        [BindProperty]
        public int? Level2Category { get; set; }
        [BindProperty]
        public int? Level3Category { get; set; }

        [FromForm]
        public string Action { get; set; }


        [BindProperty]
        public Input FormInput { get; set; }
        public IList<site.Data.Category> CategoriesHierarchy { get; private set; }

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
                Specifications = product.Specifications,
                Status = product.Status
            };

            await LoadDropdownCategories();

            var categParentsChildren =  GetLevelsFromHierarchies(product);
            FormInput.CategoryId = categParentsChildren[0].Id;
            Level2Category = categParentsChildren.Count > 1 ? categParentsChildren[1].Id : (int?) null;
            Level3Category = categParentsChildren.Count > 2 ? categParentsChildren[2].Id : (int?) null;

            return Page();
        }

        private List<Data.Category> GetLevelsFromHierarchies(Shared.Entities.Product product)
        {
            var curCategory = CategoriesHierarchy.First(e => e.Id == product.CategoryId);
            if (curCategory.Parent == null) return new List<Data.Category> { curCategory };

            var parent = CategoriesHierarchy.Where(e => e.Id == curCategory.Parent.Id).First();
            var children = CategoriesHierarchy.Where(e => e.Parent?.Id == product.CategoryId);

            var hasChildren = children.Any();
            if (hasChildren || parent.Parent == null) return new List<Data.Category> { parent, curCategory };

            return new List<Data.Category> { parent.Parent, parent, curCategory };
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var categories = await _siteContentProvider.GetAllCategories();
            var isRenderedAsService = StoreUtil.IsServiceCategory(FormInput.CategoryId, categories.ToList());

            if (Action == "deleted") return await OnDeleteAsync(id, isRenderedAsService);

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

            var categoryId = FormInput.CategoryId;
            if (Level2Category.HasValue) categoryId = Level2Category.Value;
            if (Level3Category.HasValue) categoryId = Level3Category.Value;

            var product = new Shared.Entities.Product
            {
                Id = id,
                Name = FormInput.Name,
                ProductDetails = FormInput.Description,
                Price = FormInput.Price,
                OldPrice = FormInput.Price,
                CategoryId = categoryId,
                ImageUrl = productImagePaths[0],
                SmallImageUrl = productImagePaths[0],
                StoreId = currentUser.Store.Id,
                Specifications = FormInput.Specifications,
                RenderedAsService = isRenderedAsService,
                OtherImageUrlsJson = JsonConvert.SerializeObject(productImagePaths),
                Status = FormInput.Status
            };

            try
            {
                await _productRepository.Update(product);
                var url = product.RenderedAsService ? "/Profile/Store?filter=Services" : "/Profile/Store";
                return Redirect(url);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unable to save the record: {ex.Message}. Please try again");
                _logger.LogDebug(ex.Message);
            }

            await LoadDropdownCategories();

            return Page();
        }

        private async Task<IActionResult> OnDeleteAsync(int id, bool isService)
        {
            var url = isService ? "/Profile/Store?filter=Services" : "/Profile/Store";
            await _productRepository.Delete(id);
            return Redirect(url);
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
            CategoriesHierarchy = await _siteContentProvider.GetAllCategories();

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

            StatusList = new SelectList(new[] { ProductStatuses.Active, ProductStatuses.Inactive }, FormInput.Status);
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
            public List<IFormFile >ImageUpload { get; set; }
            public Shared.ProductStatuses Status { get; set; }
        }
    }
}