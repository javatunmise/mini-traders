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
        private readonly CategoriesRepository _categoriesRepo;

        public CacheSiteContentProvider(LocationRepository locationRepository, CategoriesRepository categoriesRepository)
        {
            _locationRepo = locationRepository;
            _categoriesRepo = categoriesRepository;
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

        public async Task<IList<Category>> GetAllCategories()
        {
            var categories = (await _categoriesRepo.GetCategories())
                .Select(e => new Category
                {
                    Name = e.Name,
                    Id = e.Id,
                    Parent = e.ParentId.HasValue ? new Category { Id = e.ParentId.Value } : null
                }).ToList();

            foreach(var cat in categories)
            {
                if (cat.Parent != null && string.IsNullOrEmpty(cat.Parent.Name))
                    cat.Parent.Name = categories.SingleOrDefault(c => c.Id == cat.Parent.Id)?.Name;
            }

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