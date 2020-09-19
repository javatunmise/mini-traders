using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.Entities
{
    public class Product
    {
        public Product()
        {
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductDetails { get; set; }
        public string Specifications { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string OtherImageUrlsJson { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public ProductStatuses Status { get; set; }
        public bool RenderedAsService { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public double ReviewsCount { get; set; }
        public double AverageRating { get; set; }

        public ICollection<ProductTag> ProductTags { get; set; }

        [NotMapped]
        public List<string> OtherImageUrls { get; set; }
    }

}

namespace Shared
{
    public enum ProductStatuses
    {
        Inactive, Active, Deactivated
    }
}