using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Entities;
using site.Repositories;

namespace site.Pages.Admin.Stores
{
    public class StoreProductsModel : PageModel
    {
        private readonly ProductsRepository productRepository;
        private readonly StoreRepository storeRepository;

        public StoreProductsModel(ProductsRepository productRepository,
                StoreRepository storeRepository)
        {
            this.productRepository = productRepository;
            this.storeRepository = storeRepository;
        }
        public async Task<IActionResult> OnGet(int id, [FromQuery] ProductSearchQuery query)
        {
            CurrentStore = await storeRepository.GetStoreById(id);

            IEnumerable<Product> products = query.FilterByServices ? await productRepository.GetServices(id) :
                                                                     await productRepository.GetProducts(id);

            Products = products;
            SearchQuery = query;

            return Page();
        }


        public IEnumerable<Product> Products { get; private set; }
        public ProductSearchQuery SearchQuery { get; set; }
        public Store CurrentStore { get; private set; }
    }
    public class ProductSearchQuery
    {
        public string Filter { get; set; }
        public bool FilterByServices => Filter?.ToLower() == "services";
    }
}
