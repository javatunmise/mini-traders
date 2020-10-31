using System.Collections.Generic;
using System.Linq;

namespace Shared.ViewModels
{
    public class ProductView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductDetails { get; set; }
        public string Specifications { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string OtherImageUrlsJson { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int StoreId { get; set; }
        public int CategoryId {get;set;}
        public string CategoryName {get;set;}

        //Vendor Info
        public string VendorName { get; set; }
        public string VendorPhoneNumber { get; set; }
        public string VendorDetails { get; set; }
        public string VendorLocationId { get; set; }
        public string VendorLocationName { get; set; }
        public string VendorSubLocationName { get; set; }
        public string VendorLocation
        {
            get
            {
                return string.IsNullOrEmpty(VendorSubLocationName) ? VendorLocationName : $"{VendorLocationName}, {VendorSubLocationName}";
            }
        }
        public string VendorLogoPath { get; set; }
        public double AverageRating { get; set; }
        public double ReviewsCount { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ProductSpecList => GetProductSpecifications(Specifications);

        private IEnumerable<KeyValuePair<string, string>> GetProductSpecifications(string specifications)
        {
            if (string.IsNullOrWhiteSpace(specifications))
                return new List<KeyValuePair<string, string>>();

            return from line in specifications.Split('\n')
                   let pair = line.Split(':')
                   where pair.Length == 2
                   select new KeyValuePair<string, string>(pair[0], pair[1]);
        }
    }
}
