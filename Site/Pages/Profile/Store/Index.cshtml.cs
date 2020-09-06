using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Entities;
using site.Pages.Categories;
using site.Repositories;

namespace site.Pages.Profile.Store
{
    public class IndexModel : PageModel
    {
        private readonly AccountRepository _accountRepo;
        private readonly ProductsRepository _productRepository;

        public IndexModel(AccountRepository accountRepository, ProductsRepository productRepository)
        {
            _accountRepo = accountRepository;
            SearchQuery = new ProductSearchQuery();
            _productRepository = productRepository;
        }

        public async Task<IActionResult> OnGet(ProductSearchQuery query)
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var store = currentUser.Store;
            IEnumerable<Product> products = query.FilterByServices ? await _productRepository.GetServices(store.Id) :
                                                                     await _productRepository.GetProducts(store.Id);

            Products = products;
            SearchQuery = query;
            return Page();
        }

        public IEnumerable<Product> Products { get; private set; }

        public ProductSearchQuery SearchQuery { get; set; }
    }

    public class ProductSearchQuery
    {
        public bool FilterByServices { get; set; }
    }
}