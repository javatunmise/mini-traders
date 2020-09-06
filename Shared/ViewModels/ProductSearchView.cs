using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels
{
    public class ProductSearchView
    {
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public int RecordCount { get; set; }
        public List<Data> Records { get; set; }

        public class Data
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public string SmallImageUrl { get; set; }
            public decimal Price { get; set; }
            public decimal OldPrice { get; set; }
            public int StoreId { get; set; }
            public string StoreName { get; set; }
            public int CategoryId { get; set; }
            public double AverageRating { get; set; }
            public int CampusId { get; set; }
        }
    }


    public class ProductSearchFilter
    {
        public int? CategoryId { get; set; }
        public string SearchText { get; set; }
        public int? LocationId { get; set; }
        public int? SubLocationId { get; set; }
        public int PriceMin { get; set; }
        public int? PriceMax { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string Sort { get; set; }
    }
}
