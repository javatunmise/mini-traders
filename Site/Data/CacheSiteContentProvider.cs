using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data
{
    public class CacheSiteContentProvider : ISiteContentProvider
    {
        public async Task<IEnumerable<Carousel>> GetCarousel()
        {
            var data = new List<Carousel>
            {
                new Carousel { Id = 1, Name = "First Slide", Type = CarouselType.MainScroller,
                DestinationUrl = "/categories/1-123", ImageUrl = "/images/banner_1.png" },
                new Carousel { Id = 1, Name = "Second Slide", Type = CarouselType.MainScroller,
                DestinationUrl = "/categories/4-456", ImageUrl = "/images/banner_2.png" },
                new Carousel { Id = 1, Name = "Third Slide", Type = CarouselType.MainScroller,
                DestinationUrl = "/categories/9-900", ImageUrl = "/images/banner_3.png" },
            };

            await Task.CompletedTask;

            return data;
        }

        public Task<IEnumerable<MarketLocation>> GetLocations()
        {
            string[] items = new[] { "UniversityofLagos", "CovenantUniversity", "UniversityofNigeria", "ObafemiAwolowoUniversity", "UniversityofIlorin", "AhmaduBelloUniversity", "UniversityofAbuja", "FederalUniversityofTechnology,Akure", "UniversityofIbadan", "FederalUniversityofTechnology,Minna", "FederalUniversityofAgriculture,Abeokuta", "AfeBabalolaUniversity", "RiversStateUniversity", "LadokeAkintolaUniversityofTechnology", "BayeroUniversityKano", "UniversityofJos", "LandmarkUniversity", "FUTO", "FederalUniversity,Oye-Ekiti", "AbubakarTafawaBalewaUniversity", "Uni-Uyo", "LASU", "Uni-Ben", "OlabisiOnabanjoUniversity", "NnamdiAzikiweUniversity" };

            return Task.FromResult(items.Select((loc, ind) => new MarketLocation { Id = ind + 1, Name = loc }));
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
            var categories = new List<Category>();
            categories.Add(new Category { Id = 1, Name = "Men's Wear" });
            categories.Add(new Category { Id = 2, Name = "Women's Wear" });
            var electronics = new Category { Id = 3, Name = "Electronics", IconClass = "laptop"  };
            categories.Add(electronics);

            var tvs = new Category { Id = 17, Parent = electronics, Name = "TVs" };
            categories.Add(tvs);

            await Task.CompletedTask;

            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = new List<Category>();

            var mensWears = new Category { Id = 1, Name = "Men's Wear" };
            categories.Add(new Category { Id = 2, Name = "Women's Wear" });

            var electronics = new Category { Id = 3, Name = "Electronics", IconClass = "laptop" };
            categories.Add(electronics);
            categories.Add(new Category { Id = 4, Name = "Children's Wears" });
            categories.Add(new Category { Id = 5, Name = "Office Wears" });

            categories.Add(mensWears);

            var shoes = new Category { Id = 6, Parent = mensWears, Name = "Shoes" };
            categories.Add(shoes);
            categories.Add(new Category { Id = 7, Parent = shoes, Name = "Sneakers" });


            categories.Add(new Category { Id = 8, Parent = mensWears, Name = "Shirts" });

            var suits = new Category { Id = 9, Parent = mensWears, Name = "Suits" };
            categories.Add(suits);

            categories.Add(new Category { Id = 10, Parent = suits, Name = "Single breasted" });
            categories.Add(new Category { Id = 11, Parent = suits, Name = "Tuxedos" });
            categories.Add(new Category { Id = 12, Parent = suits, Name = "Dinner Suits" });
            categories.Add(new Category { Id = 13, Parent = suits, Name = "Double breasted" });

            var trousers = new Category { Id = 14, Parent = mensWears, Name = "Jeans Trousers" };
            categories.Add(trousers);

            categories.Add(new Category { Id = 15, Parent = trousers, Name = "Pant Trousers" });
            categories.Add(new Category { Id = 16, Parent = trousers, Name = "Pin-stripe" });

            var tvs = new Category { Id = 17, Parent = electronics, Name = "TVs" };
            categories.Add(tvs);
            categories.Add(new Category { Id = 18, Parent = electronics, Name = "Home Theater" });


            categories.Add(new Category { Id = 19, Parent = tvs, Name = "Plasma TV" });
            categories.Add(new Category { Id = 20, Parent = tvs, Name = "LED TV" });

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