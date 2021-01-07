using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Entities;
using Shared.ViewModels;
using site.Data;
using site.Repositories;
using Site.Data;

namespace site.Pages.Products
{
    public class IndexModel : PageModel
    {
        public ProductView Product { get; private set; }
        public List<string> OtherImageUrls { get; private set; }
        public Vendor Vendor { get; private set; }
        public List<Review> ProductReviews { get; set; }
        public List<Product> RelatedProducts { get; set; }
        private readonly ProductsRepository _productRepository;
        private readonly ISiteContentProvider _provider;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ProductsRepository productsRepository, ISiteContentProvider provider, ApplicationDbContext context)
        {
            _productRepository = productsRepository;
            _provider = provider;
            _dbContext = context;
        }

        public async Task<IActionResult> OnGet([FromRoute] int id, int flash_id = 0)
        {
            Product = await _productRepository.GetProductView(id);
            if (Product == null)
            {
                return RedirectToPage("/Error404");
            }

            if(flash_id > 0)
            {
                var curFlashDeal = await _provider.GetCurrentFlashDeal();
                if(curFlashDeal != null)
                {
                    var dealProduct = await _dbContext.FlashDealProducts.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == id && x.FlashDealId == flash_id);
                    if(dealProduct != null)
                    {
                        Product.OldPrice = dealProduct.OldPrice;
                        Product.Price = dealProduct.CurrentPrice;
                    }
                }
            }

            OtherImageUrls = JsonConvert.DeserializeObject<List<string>>(Product.OtherImageUrlsJson ?? "[]");

            Vendor = new Vendor
            {
                VendorId = Product.StoreId,
                Name = Product.VendorName,
                LogoUrl = Product.VendorLogoPath,
                Contact = new VendorContact { PhoneNumber = Product.VendorPhoneNumber }
            };

            ProductReviews = (await _productRepository.GetProductReviews(id))
                             .Take(5)
                             .Select(e => new Review
                             {
                                 Feedback = e.Message,
                                 ProfilePicturePath = e.ReviewerProfileImage,
                                 Rating = (int)Math.Round(e.Rating),
                                 ReviewerName = e.ReviewerName,
                                 Date = e.CreatedOn
                             }).ToList();

            RelatedProducts = (await _productRepository.GetRelatedProducts(Product.Name, Product.ProductDetails)).ToList();

            return Page();
        }

    }
}