using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Data;

namespace site.Pages.Products
{
    public class IndexModel : PageModel
    {
        public Product Product { get; private set; }
        public Vendor Vendor { get; }
        public List<Review> ProductReviews { get; set; }
        public List<Product> RelatedProducts { get; set; }

        public IndexModel()
        {
            Product = new Product
            {
                Name = "Exclusive Kayshore 6 Seater With Free Throws",
                OldPrice = 125M,
                Price = 120M,
                SmallImageUrl = "images/products/bed.jpg",
                BigImageUrl = "images/products/bed.jpg",
                ImagesUrls = new List<string> { "images/products/bed.jpg" },
                AverageRating = 4.5,
                ReviewsCount = 10,

                ProductDetails = @"Give your bedroom a lovely look with this Executive 6\'×6\' feet bedframe with two bedside and a 3 layered dressing table.
            This 6 feet x 6 feet dark brown bed with white patterned headboard was designed in the comtemporary style with its protuding forms(not just flat as in regular bedframes)  thus giving an impression of floating.Bed comes with 2 detachable bedside drawers(each with two layers of drawer).The bed was built with consideration given to style and strength majorly.Comes with an independent upholstered solid wooden weight support.Does not include mattress,
                pillows or decorThis furniture has an interesting and functional compartmentalizationto meet all your desire or purposes.It produces that touch of class, elegance and structure to your livingspace, bed room and office.It is durable and made with highest attention to details
            Headboard height is 3ft(900mm)Product is already coupled and ready for use on deliveryMade from quality particle board and ready-to-use on deliveryMaintenance: Just clean with a slightly wet napkin",

                OtherFeatures = @"SKU: UN970HL0AJLV4NAFAMZ
                                    Area of Use: Bedroom
                                    Style: Contemporary
                                    Color: Many Colours
                                    Product Line: PAWA FURNITURE
                                    Size(L x W x H cm): 6x6 feet
                                    Weight(kg): 110",
            };

            Vendor = new Vendor
            {
                Name = "Charlie's Facials & Beauties",
                Contact = new VendorContact
                {
                    PhoneNumber = "08032993003",
                    EmailAddress = "charliesbeauties@yahoo.com"
                }
            };

            ProductReviews = new List<Review>()
            {
                new Review { Title = "Cool", Feedback = "Nothing to say", ReviewerName = "Shade"},
                new Review { Title = "Not happy", Feedback = "Not good quality", ReviewerName = "Alex James"},
            };
        }

        public void OnGet()
        {

        }
    }
}