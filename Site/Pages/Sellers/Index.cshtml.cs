using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Entities;
using site.Data;
using site.Repositories;

namespace site.Pages.Sellers
{
    public class IndexModel : PageModel
    {
        private readonly ProductsRepository _productsRepo;
        private readonly StoreRepository _storeRepo;

        public IndexModel(ProductsRepository productsRepository, StoreRepository storeRepository)
        {
            _productsRepo = productsRepository;
            _storeRepo = storeRepository;
        }

        public Vendor Vendor { get; private set; }
        public IEnumerable<Product> Products { get; private set; }
        public double VendorRating { get; private set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null) return Redirect("/search");

            var store = await _storeRepo.GetStoreById(id.Value);
            if (store == null) return Redirect("/search");

            Vendor = new Vendor
            {
                VendorId = store.Id,
                Name = store.StoreName,
                LogoUrl = store.LogoPath,
                Contact = new VendorContact { PhoneNumber = store.PhoneNumber }
            };

            Products = await _productsRepo.GetAllProducts(id.Value);

            VendorRating = Products.Sum(e => e.AverageRating) / Products.Count();

            return Page();
        }
    }
}