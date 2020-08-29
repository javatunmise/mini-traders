using Microsoft.AspNetCore.Mvc.ViewFeatures;
using site.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data
{
    public class CacheSiteContentProvider : ISiteContentProvider
    {
        private readonly LocationRepository _locationRepo;

        public CacheSiteContentProvider(LocationRepository locationRepository)
        {
            _locationRepo = locationRepository;
        }

        public async Task<IEnumerable<Carousel>> GetCarousel()
        {
            var data = new List<Carousel>
            {
                new Carousel { Id = 1, Name = "First Slide", Type = CarouselType.MainScroller,
                DestinationUrl = "/categories/1-123", ImageUrl = "CONFORT.png" },
                new Carousel { Id = 1, Name = "Second Slide", Type = CarouselType.MainScroller,
                DestinationUrl = "/categories/4-456", ImageUrl = "Few Second.png" },
            };

            await Task.CompletedTask;

            return data;
        }

        public async Task<IEnumerable<MarketLocation>> GetLocations()
        {
            var campuses = (await _locationRepo.GetCampuses())
                            .Select((loc, ind) => new MarketLocation { Id = loc.Id, Name = loc.Name })
                            .ToList();

            return campuses;
        }

        public async Task<IEnumerable<MarketLocation>> GetSubLocations(int campusId)
        {
            var hostels = (await _locationRepo.GetHostels(campusId))
                            .Select((loc, ind) => new MarketLocation { Id = loc.Id, Name = loc.Name })
                            .ToList();

            return hostels;
        }

        public async Task<IEnumerable<CompanyLogo>> GetSitePartnersLogos()
        {
            await Task.CompletedTask;
            return new List<CompanyLogo> {
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://www.logodesignteam.com/images/portfolio-images/food-beverages-logo-design/food-beverages-logo-design-new10.jpg",
                    CompanySiteUrl = "http://topbrands.com"
                },
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://www.logodesignteam.com/images/portfolio-images/food-beverages-logo-design/food-beverages-logo-design-new6.jpg",
                    CompanySiteUrl = "http://topbrands.com"
                },
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://www.kindpng.com/picc/m/112-1122331_fresh-fit-foods-fresh-healthy-food-logo-png.png",
                    CompanySiteUrl = "http://topbrands.com"
                },
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://www.logodesignteam.com/images/portfolio-images/food-beverages-logo-design/food-beverages-logo-design-new10.jpg",
                    CompanySiteUrl = "http://topbrands.com"
                },
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://cdn2.vectorstock.com/i/1000x1000/74/06/fried-chicken-restaurant-logo-template-vector-28277406.jpg",
                    CompanySiteUrl = "http://topbrands.com"
                },
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://www.deluxe.com/sites/deluxe.signupserver.com/files/styles/square_185_185/public/mygirls-logo.jpg",
                    CompanySiteUrl = "http://topbrands.com"
                },
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://www.deluxe.com/sites/deluxe.signupserver.com/files/styles/square_185_185/public/mygirls-logo.jpg",
                    CompanySiteUrl = "http://topbrands.com"
                },
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://www.logodesignteam.com/images/portfolio-images/food-beverages-logo-design/food-beverages-logo-design12.jpg",
                    CompanySiteUrl = "http://topbrands.com"
                },
                new CompanyLogo
                {
                    CompanyLogoUrl = "https://www.logopik.com/wp-content/uploads/edd/2018/06/Food-and-Beverage-Logo.png",
                    CompanySiteUrl = "http://topbrands.com"
                }
            };
        }

        public async Task<IEnumerable<Category>> GetSiteTopCategories()
        {
            var categories = (await GetAllCategories()).Where(c => c.Parent == null);

            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = new List<Category>
            {

new Category { Id = 1, Parent = null, Name = "Services" },
new Category { Id = 10, Parent = new Category { Id = 1 }, Name = "Creative Services" },
new Category { Id = 101, Parent = new Category { Id = 10 }, Name = "Makeup artists" },
new Category { Id = 102, Parent = new Category { Id = 10 }, Name = "Hair Styling" },
new Category { Id = 103, Parent = new Category { Id = 10 }, Name = "Haircut" },
new Category { Id = 104, Parent = new Category { Id = 10 }, Name = "Pedicure and manicure" },
new Category { Id = 105, Parent = new Category { Id = 10 }, Name = "Nails fixing" },
new Category { Id = 106, Parent = new Category { Id = 10 }, Name = "Voucher/Data vendors" },
new Category { Id = 107, Parent = new Category { Id = 10 }, Name = "Gift Cards" },
new Category { Id = 108, Parent = new Category { Id = 10 }, Name = "Fashion Services" },
new Category { Id = 109, Parent = new Category { Id = 10 }, Name = "Photograph and Video Shooting/Editing" },
new Category { Id = 1091, Parent = new Category { Id = 10 }, Name = "Graphics and Printing Services" },
new Category { Id = 1092, Parent = new Category { Id = 10 }, Name = "Surprise planning & Gifting" },
new Category { Id = 1093, Parent = new Category { Id = 10 }, Name = "Shoe making" },
new Category { Id = 1094, Parent = new Category { Id = 10 }, Name = "Events planning & Development" },
new Category { Id = 1095, Parent = new Category { Id = 10 }, Name = "Disk Jockeys" },
new Category { Id = 1096, Parent = new Category { Id = 10 }, Name = "Ushering services" },
new Category { Id = 1097, Parent = new Category { Id = 10 }, Name = "Modelling services" },
new Category { Id = 1098, Parent = new Category { Id = 10 }, Name = "Rental Services" },
new Category { Id = 1099, Parent = new Category { Id = 10 }, Name = "Web design and Development" },
new Category { Id = 1081, Parent = new Category { Id = 10 }, Name = "Hosting Services" },
new Category { Id = 11, Parent = new Category { Id = 1 }, Name = "Catering & Food Services" },
new Category { Id = 111, Parent = new Category { Id = 11 }, Name = "On-demand kitchen" },
new Category { Id = 112, Parent = new Category { Id = 11 }, Name = "Event Catering Services" },
new Category { Id = 113, Parent = new Category { Id = 11 }, Name = "Cake Services" },
new Category { Id = 114, Parent = new Category { Id = 11 }, Name = "Small Chops/Snacks" },
new Category { Id = 115, Parent = new Category { Id = 11 }, Name = "Chapman/Cocktails/Parfait/Smoothies" },
new Category { Id = 116, Parent = new Category { Id = 11 }, Name = "Food Vendors" },
new Category { Id = 12, Parent = new Category { Id = 1 }, Name = "Tutorial and Training Services" },
new Category { Id = 121, Parent = new Category { Id = 12 }, Name = "Academic tutors" },
new Category { Id = 122, Parent = new Category { Id = 12 }, Name = "Cake training" },
new Category { Id = 123, Parent = new Category { Id = 12 }, Name = "Fitness training" },
new Category { Id = 124, Parent = new Category { Id = 12 }, Name = "Cooking training" },
new Category { Id = 125, Parent = new Category { Id = 12 }, Name = "Fashion training" },
new Category { Id = 126, Parent = new Category { Id = 12 }, Name = "Hair training" },
new Category { Id = 127, Parent = new Category { Id = 12 }, Name = "Driving training" },
new Category { Id = 128, Parent = new Category { Id = 12 }, Name = "Music training" },
new Category { Id = 129, Parent = new Category { Id = 12 }, Name = "Others" },
new Category { Id = 13, Parent = new Category { Id = 1 }, Name = "Content Creation" },
new Category { Id = 131, Parent = new Category { Id = 13 }, Name = "Academic & technical writing" },
new Category { Id = 132, Parent = new Category { Id = 13 }, Name = "Editing" },
new Category { Id = 133, Parent = new Category { Id = 13 }, Name = "Caption writers" },
new Category { Id = 134, Parent = new Category { Id = 13 }, Name = "CV writers" },
new Category { Id = 14, Parent = new Category { Id = 1 }, Name = "Repair and Maintenance Services" },
new Category { Id = 141, Parent = new Category { Id = 14 }, Name = "Phones and Tablets repairs" },
new Category { Id = 142, Parent = new Category { Id = 14 }, Name = "Laptop repairs" },
new Category { Id = 143, Parent = new Category { Id = 14 }, Name = "Laundry Services" },
new Category { Id = 2, Parent = null, Name = "Gadgets & Accessories" },
new Category { Id = 20, Parent = new Category { Id = 2 }, Name = "Mobile Phones" },
new Category { Id = 21, Parent = new Category { Id = 2 }, Name = "Tablets" },
new Category { Id = 22, Parent = new Category { Id = 2 }, Name = "Mobile Phone Accessories" },
new Category { Id = 23, Parent = new Category { Id = 2 }, Name = "Tablet Accessories" },
new Category { Id = 24, Parent = new Category { Id = 2 }, Name = "Laptops " },
new Category { Id = 25, Parent = new Category { Id = 2 }, Name = "Laptop Accessories" },
new Category { Id = 3, Parent = null, Name = "Health & Beauty" },
new Category { Id = 31, Parent = new Category { Id = 3 }, Name = "Fragrance" },
new Category { Id = 311, Parent = new Category { Id = 31 }, Name = "Women's Perfume" },
new Category { Id = 312, Parent = new Category { Id = 31 }, Name = "Men's Perfume" },
new Category { Id = 313, Parent = new Category { Id = 31 }, Name = "Perfume oil" },
new Category { Id = 314, Parent = new Category { Id = 31 }, Name = "body spray" },
new Category { Id = 315, Parent = new Category { Id = 31 }, Name = "Personal Care" },
new Category { Id = 32, Parent = new Category { Id = 3 }, Name = "Hair" },
new Category { Id = 321, Parent = new Category { Id = 32 }, Name = "Wigs" },
new Category { Id = 322, Parent = new Category { Id = 32 }, Name = "Attachment" },
new Category { Id = 323, Parent = new Category { Id = 32 }, Name = "Crochet" },
new Category { Id = 324, Parent = new Category { Id = 32 }, Name = "Weaves" },
new Category { Id = 325, Parent = new Category { Id = 32 }, Name = "Extension" },
new Category { Id = 326, Parent = new Category { Id = 32 }, Name = "Closure" },
new Category { Id = 327, Parent = new Category { Id = 32 }, Name = "Hair products" },
new Category { Id = 328, Parent = new Category { Id = 32 }, Name = "Hair accessories" },
new Category { Id = 33, Parent = new Category { Id = 3 }, Name = "Makeup products" },
new Category { Id = 331, Parent = new Category { Id = 33 }, Name = "Foundation" },
new Category { Id = 332, Parent = new Category { Id = 33 }, Name = "Powder" },
new Category { Id = 333, Parent = new Category { Id = 33 }, Name = "Eye shadow" },
new Category { Id = 334, Parent = new Category { Id = 33 }, Name = "Contour kit" },
new Category { Id = 335, Parent = new Category { Id = 33 }, Name = "Eye mascara and liner" },
new Category { Id = 336, Parent = new Category { Id = 33 }, Name = "Pencils" },
new Category { Id = 337, Parent = new Category { Id = 33 }, Name = "Lip stick & gloss" },
new Category { Id = 338, Parent = new Category { Id = 33 }, Name = "Makeup Accessories" },
new Category { Id = 339, Parent = new Category { Id = 33 }, Name = "Primer" },
new Category { Id = 340, Parent = new Category { Id = 33 }, Name = "Face wipes" },
new Category { Id = 341, Parent = new Category { Id = 33 }, Name = "Eye lashes" },
new Category { Id = 342, Parent = new Category { Id = 33 }, Name = "Contact lenses" },
new Category { Id = 343, Parent = new Category { Id = 33 }, Name = "Make-up brushes" },
new Category { Id = 4, Parent = null, Name = "Facial & Skincare" },
new Category { Id = 41, Parent = new Category { Id = 4 }, Name = "Soap" },
new Category { Id = 42, Parent = new Category { Id = 4 }, Name = "Scrub" },
new Category { Id = 43, Parent = new Category { Id = 4 }, Name = "Mask" },
new Category { Id = 44, Parent = new Category { Id = 4 }, Name = "Creme" },
new Category { Id = 45, Parent = new Category { Id = 4 }, Name = "Oil" },
new Category { Id = 46, Parent = new Category { Id = 4 }, Name = "Organics" },
new Category { Id = 47, Parent = new Category { Id = 4 }, Name = "Skin care accessories" },
new Category { Id = 5, Parent = null, Name = "Grocery & Kitchen Items" },
new Category { Id = 51, Parent = new Category { Id = 5 }, Name = "Food and beverages" },
new Category { Id = 52, Parent = new Category { Id = 5 }, Name = "Snacks" },
new Category { Id = 53, Parent = new Category { Id = 5 }, Name = "Soaps &Antiseptics" },
new Category { Id = 54, Parent = new Category { Id = 5 }, Name = "Kitchen items" },
new Category { Id = 541, Parent = new Category { Id = 54 }, Name = "Kitchen utensils" },
new Category { Id = 542, Parent = new Category { Id = 54 }, Name = "Kitchen Electrical Equipments " },
new Category { Id = 6, Parent = null, Name = "USED ITEMS" },
new Category { Id = 61, Parent = new Category { Id = 6 }, Name = "Laptops" },
new Category { Id = 62, Parent = new Category { Id = 6 }, Name = "Mobile phone and Laptop Accessories" },
new Category { Id = 63, Parent = new Category { Id = 6 }, Name = "Home and kitchen items" },
new Category { Id = 64, Parent = new Category { Id = 6 }, Name = "Home Electrical Appliances" },
new Category { Id = 65, Parent = new Category { Id = 6 }, Name = "Fashion" },
new Category { Id = 66, Parent = new Category { Id = 6 }, Name = "Books" },
new Category { Id = 67, Parent = new Category { Id = 6 }, Name = "Sporting goods" },
new Category { Id = 68, Parent = new Category { Id = 6 }, Name = "Beauty and skin care products" },
new Category { Id = 69, Parent = new Category { Id = 6 }, Name = "Others" },
new Category { Id = 7, Parent = null, Name = "Fashion & clothing" },
new Category { Id = 71, Parent = new Category { Id = 7 }, Name = "Guys clothing" },
new Category { Id = 711, Parent = new Category { Id = 71 }, Name = "shirts" },
new Category { Id = 712, Parent = new Category { Id = 71 }, Name = "T-shirts" },
new Category { Id = 713, Parent = new Category { Id = 71 }, Name = "polos" },
new Category { Id = 714, Parent = new Category { Id = 71 }, Name = "jeans" },
new Category { Id = 715, Parent = new Category { Id = 71 }, Name = "suits/blazers/jackets" },
new Category { Id = 716, Parent = new Category { Id = 71 }, Name = "trousers/shorts" },
new Category { Id = 72, Parent = new Category { Id = 7 }, Name = "Ladies clothing" },
new Category { Id = 721, Parent = new Category { Id = 72 }, Name = "dresses" },
new Category { Id = 722, Parent = new Category { Id = 72 }, Name = "tops " },
new Category { Id = 723, Parent = new Category { Id = 72 }, Name = "trousers" },
new Category { Id = 724, Parent = new Category { Id = 72 }, Name = "skirts" },
new Category { Id = 725, Parent = new Category { Id = 72 }, Name = "jumpsuits&playsuits" },
new Category { Id = 726, Parent = new Category { Id = 72 }, Name = "suits & blazers" },
new Category { Id = 727, Parent = new Category { Id = 72 }, Name = "Lingerie" },
new Category { Id = 73, Parent = new Category { Id = 7 }, Name = "Guys Footwear" },
new Category { Id = 74, Parent = new Category { Id = 7 }, Name = "Ladies Footwear" },
new Category { Id = 75, Parent = new Category { Id = 7 }, Name = "Underwear/Pyjamas" },
new Category { Id = 751, Parent = new Category { Id = 75 }, Name = "male underwear" },
new Category { Id = 752, Parent = new Category { Id = 75 }, Name = "female underwear" },
new Category { Id = 753, Parent = new Category { Id = 75 }, Name = "children underwear" },
new Category { Id = 754, Parent = new Category { Id = 75 }, Name = "male pyjamas" },
new Category { Id = 755, Parent = new Category { Id = 75 }, Name = "female pyjamas" },
new Category { Id = 76, Parent = new Category { Id = 7 }, Name = "Bags" },
new Category { Id = 77, Parent = new Category { Id = 7 }, Name = "Textiles" },
new Category { Id = 78, Parent = new Category { Id = 7 }, Name = "Accessories" },
new Category { Id = 782, Parent = new Category { Id = 78 }, Name = "guys' watches" },
new Category { Id = 783, Parent = new Category { Id = 78 }, Name = "ladies' watches" },
new Category { Id = 784, Parent = new Category { Id = 78 }, Name = "unisex watches" },
new Category { Id = 785, Parent = new Category { Id = 78 }, Name = "Necklaces " },
new Category { Id = 786, Parent = new Category { Id = 78 }, Name = "Rings" },
new Category { Id = 787, Parent = new Category { Id = 78 }, Name = "Bracelets" },
new Category { Id = 788, Parent = new Category { Id = 78 }, Name = "Beads" },
new Category { Id = 789, Parent = new Category { Id = 78 }, Name = "Waist & leg chains" },
new Category { Id = 7890, Parent = new Category { Id = 78 }, Name = "Scarf & caps" },
new Category { Id = 7891, Parent = new Category { Id = 78 }, Name = "Eyewear & sunglasses" },
new Category { Id = 79, Parent = new Category { Id = 7 }, Name = "Beddings" },
new Category { Id = 791, Parent = new Category { Id = 79 }, Name = "Bedsheets" },
new Category { Id = 792, Parent = new Category { Id = 79 }, Name = "Pillow cases" },
new Category { Id = 793, Parent = new Category { Id = 79 }, Name = "Duvet covers" },
new Category { Id = 790, Parent = new Category { Id = 79 }, Name = "Others" },
new Category { Id = 8, Parent = null, Name = "Sports & Lifestyle" },
new Category { Id = 81, Parent = new Category { Id = 8 }, Name = "Sport wears" },
new Category { Id = 811, Parent = new Category { Id = 81 }, Name = "Joggers" },
new Category { Id = 812, Parent = new Category { Id = 81 }, Name = "Jerseys" },
new Category { Id = 813, Parent = new Category { Id = 81 }, Name = "Sports bra" },
new Category { Id = 814, Parent = new Category { Id = 81 }, Name = "Sports tights" },
new Category { Id = 815, Parent = new Category { Id = 81 }, Name = "Boots" },
new Category { Id = 82, Parent = new Category { Id = 8 }, Name = "Sport accessories" },
new Category { Id = 83, Parent = new Category { Id = 8 }, Name = "Gaming accessories" },
new Category { Id = 9, Parent = null, Name = "Books" },
new Category { Id = 91, Parent = new Category { Id = 9 }, Name = "Student works" },
new Category { Id = 92, Parent = new Category { Id = 9 }, Name = "Novels" },
new Category { Id = 93, Parent = new Category { Id = 9 }, Name = "Drama" },
new Category { Id = 94, Parent = new Category { Id = 9 }, Name = "Management/Business textbooks" },
new Category { Id = 95, Parent = new Category { Id = 9 }, Name = "Engineering textbooks" },
new Category { Id = 96, Parent = new Category { Id = 9 }, Name = "Social Sciences textbooks" },
new Category { Id = 97, Parent = new Category { Id = 9 }, Name = "Medical Sciences textbooks" },
new Category { Id = 98, Parent = new Category { Id = 9 }, Name = "Education textbooks" },
new Category { Id = 99, Parent = new Category { Id = 9 }, Name = "Inspiration Books" },
new Category { Id = 990, Parent = new Category { Id = 9 }, Name = "Law books" },

            };

            foreach(var cat in categories)
            {
                if (cat.Parent != null && string.IsNullOrEmpty(cat.Parent.Name))
                    cat.Parent.Name = categories.SingleOrDefault(c => c.Id == cat.Parent.Id)?.Name;
            }

            await Task.CompletedTask;

            return categories;
        }

        public async Task<Site> GetSiteInfo()
        {
            await Task.CompletedTask;
            return new Site
            {
                PhoneNumber = "+234 813 776 8881",
                    Email = "shopatfirstchoice@yahoo.com",
                    Address = "159 Olonade Yaba, Lagos",               
                FacebookUrl = "http://facebook.com/pages/first-choice-online",
                YoutubeUrl = "",
                InstagramUrl = "",
                TwitterUrl = "",
                LinkedInUrl = ""
            };
        }

        public async Task<FooterLinks> GetFooterHelpAndSupportLinks()
        {
            await Task.CompletedTask;
            return new FooterLinks("Help and Support", new List<FooterLink>
            {
                new FooterLink { Text = "My account", Url ="/pages/1022-my-account" },
                new FooterLink { Text = "Order History", Url ="/pages/1022-order-history" },
                new FooterLink { Text = "FAQ", Url ="/pages/1022-faq" },
                new FooterLink { Text = "Specials", Url ="/pages/1022-specials" },
                new FooterLink { Text = "Help Center", Url ="/pages/1022-help-center" }

            });
        }

        public async Task<FooterLinks> GetFooterCustomerServiceLinks()
        {
            await Task.CompletedTask;
            return new FooterLinks("Customer Service", new List<FooterLink>
            {
                new FooterLink { Text = "My account", Url ="/pages/1022-My account "},
                new FooterLink { Text = "Order History", Url ="/pages/1022-Order History "},
                new FooterLink { Text = "FAQ", Url ="/pages/1022-FAQ "},
                new FooterLink { Text = "Specials", Url ="/pages/1022-Specials "},
                new FooterLink { Text = "Help Center", Url ="/pages/1022-Help Center "},
            });
        }

        public async Task<FooterLinks> GetFooterCorporationSectionLinks()
        {
            await Task.CompletedTask;
            return new FooterLinks("Our Company", new List<FooterLink>
            {
                new FooterLink { Text = "About Us", Url ="/pages/1022-About Us "},
                new FooterLink { Text = "Customer Service", Url ="/pages/1022-Customer Service "},
                new FooterLink { Text = "Company", Url ="/pages/1022-Company "},
                new FooterLink { Text = "Investor Relations", Url ="/pages/1022-Investor Relations "},
                new FooterLink { Text = "Advanced Search", Url ="/pages/1022-Advanced Search "},
            });
        }

        public async Task<FooterLinks> GetFooterWhyUsLinks()
        {
            await Task.CompletedTask;
            return new FooterLinks("Why Choose Us", new List<FooterLink>
            {
                new FooterLink { Text = "Shopping Guide", Url ="/pages/1022-Shopping Guide "},
                new FooterLink { Text = "Blog", Url ="/pages/1022-Blog "},
                new FooterLink { Text = "Company", Url ="/pages/1022-Company "},
                new FooterLink { Text = "Invenstor Relations", Url ="/pages/1022-Invenstor Relations "},
                new FooterLink { Text = "Contact Us", Url ="/pages/1022-Contact Us "},
            });
        }
    }
}