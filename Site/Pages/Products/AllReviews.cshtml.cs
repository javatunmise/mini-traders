using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Shared.Entities;
using Shared.ViewModels;
using site.Data;
using site.Repositories;

namespace site.Pages.Products
{
    public class AllReviews : PageModel
    {
        public ProductView Product { get; private set; }
        public List<string> OtherImageUrls { get; private set; }
        public Vendor Vendor { get; private set; }
        public List<Review> ProductReviews { get; set; }
        public List<Product> RelatedProducts { get; set; }
        private readonly ProductsRepository _productRepository;

        public AllReviews(ProductsRepository productsRepository)
        {
            _productRepository = productsRepository;
        }

        public async Task OnGet([FromRoute] int id)
        {
            Product = await _productRepository.GetProductView(id);
            OtherImageUrls = JsonConvert.DeserializeObject<List<string>>(Product.OtherImageUrlsJson ?? "[]");

            Vendor = new Vendor
            {
                VendorId = Product.StoreId,
                Name = Product.VendorName,
                LogoUrl = Product.VendorLogoPath,
                Contact = new VendorContact { PhoneNumber = Product.VendorPhoneNumber }
            };

            ProductReviews = (await _productRepository.GetProductReviews(id))
                             .Select(e => new Review
                             {
                                 Feedback = e.Message,
                                 ProfilePicturePath = e.ReviewerProfileImage,
                                 Rating = (int)Math.Round(e.Rating),
                                 ReviewerName = e.ReviewerName,
                                 Date = e.CreatedOn
                             }).ToList();
        }

    }
}