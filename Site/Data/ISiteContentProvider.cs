using Shared.Entities;
using site.ViewComponents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace site.Data
{
    public interface ISiteContentProvider
    {
        Task<IEnumerable<CompanyLogo>> GetSitePartnersLogos();
        Task<IEnumerable<Category>> GetSiteTopCategories();
        Task<IList<Category>> GetAllCategories();
        Task<IEnumerable<MarketLocation>> GetLocations();
        Task<IEnumerable<MarketLocation>> GetSubLocations(int locationId);

        Task<IEnumerable<Carousel>> GetCarousel();
        Task<Site> GetSiteInfo();
        Task<List<Product>> GetFlashDeals();
        Task<FlashDeal> GetCurrentFlashDeal();
        Task<IEnumerable<Product>> GetRecommended(SiteUser siteUser);
        Task<Shared.Entities.Site> GetCurrentSite();
        Task<IEnumerable<Product>> GetTopServices();
        Task<List<Product>> GetLatestProducts(int categoryId, int numOfRecords);

        //Task<IEnumerable<Product>> GetFeaturedProducts();
        //Task<IEnumerable<Product>> GetNewProducts();

        Task<FooterLinks> GetFooterHelpAndSupportLinks();
        Task<FooterLinks> GetFooterCustomerServiceLinks();
        Task<FooterLinks> GetFooterCorporationSectionLinks();
        Task<FooterLinks> GetFooterWhyUsLinks();
    }

    public class FooterLinks
    {
        public FooterLinks(string title, IEnumerable<FooterLink> links)
        {
            Title = title;
            Links = links;
        }

        public string Title { get; private set; }
        public IEnumerable<FooterLink> Links { get; private set; }
    }

    public class FooterLink
    {
        public FooterLink() : this("", "")
        {
        }

        public FooterLink(string text, string url)
        {
            Text = text;
            Url = url;
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
    }

    public class Site
    {
        public Site()
        {

        }

        public Site(Shared.Entities.Site site)
        {
            if (site != null)
            {
                AboutUsIntro = site.AboutUsIntro;
                CopyRightName = site.CopyRightName;
                PhoneNumber = site.PhoneNumber;
                Email = site.Email;
                Address = site.Address;
                WorkingHours = site.WorkingHours;
                FacebookUrl = site.FacebookUrl;
                YoutubeUrl = site.YoutubeUrl;
                InstagramUrl = site.InstagramUrl;
                LinkedInUrl = site.LinkedInUrl;
                TwitterUrl = site.TwitterUrl;
                IsGoogleAnalyticsEnabled = site.IsGoogleAnalyticsEnabled;
            }
        }

        public string AboutUsIntro { get; set; }
        public string CopyRightName { get; internal set; }
        public string PhoneNumber { get; internal set; }
        public string Email { get; internal set; }
        public string Address { get; internal set; }
        public string WorkingHours { get; set; }
        public string FacebookUrl { get; internal set; }
        public string YoutubeUrl { get; internal set; }
        public string InstagramUrl { get; internal set; }
        public string LinkedInUrl { get; internal set; }
        public string TwitterUrl { get; internal set; }

        public bool IsGoogleAnalyticsEnabled { get; set; }
    }
    
    //public class Product
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public Vendor Vendor { get; set; }
    //    public string BigImageUrl { get; internal set; }
    //    public string SmallImageUrl { get; internal set; }
    //    public decimal OldPrice { get; internal set; }
    //    public decimal Price { get; internal set; }
    //    public List<string> ImagesUrls { get; internal set; }
    //    public double AverageRating { get; set; }
    //    public int ReviewsCount { get; internal set; }
    //    public string ProductDetails { get; internal set; }
    //    public string OtherFeatures { get; internal set; }
    //}


    public class Review
    {
        public string ProfilePicturePath { get; set; }
        public string ReviewerName { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public DateTime Date;
    }

    public class Vendor
    {
        public int VendorId { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public VendorContact Contact { get; set; }
    }

    public class VendorContact
    {
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }

    public class CompanyLogo
    {
        public string CompanySiteUrl { get; set; }
        public string CompanyLogoUrl { get; set; }
    }

    public class Category
    {
        public Category()
        {
            Children = new List<Category>();
        }

        public Category Parent { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Category> Children { get; set; }
        public string IconClass { get; set; }
        public string IconImagePath { get; set; }
    }

    public class MarketLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MarketLocation Parent { get; set; }
    }

    public class Carousel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string DestinationUrl { get; internal set; }
        public int Ordering { get; internal set; }
        internal CarouselType Type { get; set; }
    }


    internal enum CarouselType
    {
        MainScroller = 0
    }
}