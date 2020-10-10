using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Shared.ViewModels;
using site.Data;
using site.Repositories;

namespace site.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly ISiteContentProvider _provider;
        private readonly ProductsRepository _productsRepo;

        public IEnumerable<Category> Categories { get; private set; }
        public IEnumerable<MarketLocation> Locations { get; private set; }
        public IEnumerable<MarketLocation> SubLocations { get; private set; }
        public SearchQuery Query { get; set; }
        public (decimal Min, decimal Max) PriceRange { get; set; } = (0, 100000);
        public (int PageIndex, int TotalRecords) PagingInfo = (1, 20);
        public List<ProductSearchView.Data> Products { get; set; } = new List<ProductSearchView.Data>();

        public IndexModel(ISiteContentProvider provider, ProductsRepository productsRepository)
        {
            _provider = provider;
            _productsRepo = productsRepository;
        }


        public async Task<IActionResult> OnGet([FromQuery] SearchQuery query)
        {
            int.TryParse(query.LocationId, out int locationId);
            int.TryParse(query.SubLocationId, out int subLocationId);

            Categories = await _provider.GetSiteTopCategories();

            query.PageIndex = Math.Max(1, query.PageIndex);

            var searchResult = await _productsRepo.SearchProduct(new ProductSearchFilter
            {
                CategoryId = null,
                LocationId = locationId > 0 ? locationId : (int?)null,
                PriceMax = query.MaxPrice > 0 ? (int)Math.Ceiling(query.MaxPrice) : (int?)null,
                PriceMin = (int)query.MinPrice,
                SearchText = string.IsNullOrWhiteSpace(query.SearchText) ? null : query.SearchText,
                SubLocationId = subLocationId > 0 ? subLocationId : (int?)null,
                PageIndex = query.PageIndex,
                PageSize = 20,
                Sort = GetValidSort(query.OrderBy)
            });

            Locations = await _provider.GetLocations();
            SubLocations = await _provider.GetSubLocations(locationId);
            PriceRange = (searchResult.MinPrice, searchResult.MaxPrice);
            PagingInfo = (query.PageIndex, searchResult.RecordCount);
            Products = searchResult.Records;

            Query = query;

            return Page();
        }

        public async Task<JsonResult> OnGetSubLocations(int curLocation)
        {
            var locations = await _provider.GetLocations();
            var subLocations = locations.Where(x => x.Parent?.Id > 0 && x.Parent?.Id == curLocation);

            var json = JsonConvert.SerializeObject(subLocations);
            return new JsonResult(json);
        }

        private string GetValidSort(string orderBy)
        {
            var valid = new[] { "date", "price", "price-desc" };

            return valid.Contains(orderBy) ? orderBy : "date";
        }
    }

    public class SearchQuery
    {
        [FromQuery(Name = "location")]
        public string LocationId { get; set; }

        [FromQuery(Name = "sub_location")]
        public string SubLocationId { get; set; }

        [FromQuery(Name = "min_price")]
        public decimal MinPrice { get; set; }

        [FromQuery(Name = "max_price")]
        public decimal MaxPrice { get; set; }

        [FromQuery(Name = "q")]
        public string SearchText { get; set; }

        public int PageIndex { get; set; }
        public string OrderBy { get; set; }
    }
}