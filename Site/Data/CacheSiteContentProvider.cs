using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Entities;
using site.Data.Repositories;
using Site.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CacheSiteContentProvider> _logger;

        public CacheSiteContentProvider(LocationRepository locationRepository, 
                                        CategoriesRepository categoriesRepository, 
                                        ApplicationDbContext context,
                                        ILogger<CacheSiteContentProvider> logger)
        {
            _locationRepo = locationRepository;
            _categoriesRepo = categoriesRepository;
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Carousel>> GetCarousel()
        {
            var carousels = await _context.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Carousels)
                                                     .ToListAsync();

            var data = carousels.Select(e => new Carousel
            {
                Id = e.Id,
                Name = e.EntityId,
                Type = CarouselType.MainScroller,
                DestinationUrl = e.Link,
                ImageUrl = e.ImagePath,
                Ordering = e.Ordering
            });

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
                if(cat.Parent != null){
                    cat.Parent = categories.FirstOrDefault(e => e.Id == cat.Parent.Id);
                }		                
            }

            return categories;
        }

        public async Task<Site> GetSiteInfo()
        {
            var siteId = new Guid(Shared.Entities.Site.Identifier);
            var site = await _context.Sites.FirstOrDefaultAsync(e => e.Id == siteId);
            return new Site(site);
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

        public async Task<IEnumerable<Product>> GetTopServices()
        {
            return await _context.Products.FromSqlRaw(SQLUtil.GetTopServicesQuery()).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetRecommended(SiteUser siteUser)
        {
            var campus = new SqlParameter("campusId", System.Data.SqlDbType.Int)
            {
                Value = siteUser?.CampusId ?? 0
            };

            var query = SQLUtil.GetRecommendedQuery();
            _logger.LogInformation("QUERY: " + query);
            return await _context.Products.FromSqlRaw(query, campus).ToListAsync();
        }

        public async Task<List<Product>> GetLatestProducts(int categoryId, int numOfRecords)
        {
            return await _context.Products.Where(e => e.CategoryId == categoryId)
                         .OrderByDescending(e => e.CreatedOn)
                         .Take(numOfRecords)
                         .ToListAsync();
        }

        public async Task<List<Product>> GetFlashDeals()
        {
            await Task.CompletedTask;
            return new List<Product>();
        }

        public async Task<FlashDeal> GetCurrentFlashDeal()
        {
            await Task.CompletedTask;
            return new FlashDeal()
            {
                Name = "Easter Sales",
                StartDate = DateTime.Now.AddDays(-5),
                EndDate = DateTime.Now.AddDays(3)
            };
        }
    }
}