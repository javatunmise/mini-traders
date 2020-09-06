using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Entities;
using Shared.ViewModels;
using site.Data;
using site.Repositories;
using Category = site.Data.Category;

namespace site.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ISiteContentProvider _provider;
        private readonly ProductsRepository _productsRepo;

        public IEnumerable<Category> SubCategories { get; private set; }
        public Category Category { get; set; }
        public IEnumerable<MarketLocation> Locations { get; private set; }
        public IEnumerable<MarketLocation> SubLocations { get; private set; }
        public SearchQuery Query { get; set; }
        public (decimal Min, decimal Max) PriceRange { get; set; } = (0, 100000);
        public (int PageIndex, int TotalRecords) PagingInfo = (1, 20);
        public List<ProductSearchView.Data> Products { get; set; } = new List<ProductSearchView.Data>();
        public ProductSearchView SearchSummary = new ProductSearchView();

        public IndexModel(ISiteContentProvider provider, ProductsRepository productsRepository)
        {
            _provider = provider;
            _productsRepo = productsRepository;
        }

        public async Task<IActionResult> OnGet([FromQuery] SearchQuery query)
        {
            int.TryParse(query.LocationId, out int locationId);
            int.TryParse(query.SubLocationId, out int subLocationId);

            if (Id <= 0) return NotFound();

            var _categories = await _provider.GetAllCategories();
            var _category = _categories.SingleOrDefault(c => c.Id == Id);

            if (_category == null) return NotFound();

            Category = _category;
            SubCategories = _categories.Where(c => c.Parent?.Id == Id);
            query.PageIndex = Math.Max(1, query.PageIndex);

            var searchResult = await _productsRepo.SearchProduct(new ProductSearchFilter
            {
                CategoryId = Id,
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

        private string GetValidSort(string orderBy)
        {
            var valid = new[] { "date", "price", "price-desc" };

            return valid.Contains(orderBy) ? orderBy : "date";
        }

        public Expression<Func<Product, bool>> BuildFilter(SearchQuery query)
        {
            int.TryParse(query.LocationId, out int locationId);
            int.TryParse(query.SubLocationId, out int subLocationId);

            if(!string.IsNullOrWhiteSpace(query.SearchText))
            {
                if (locationId > 0 && subLocationId > 0 && query.MaxPrice > 0)
                {
                    return (prod) => prod.CategoryId == Id && prod.Name.StartsWith(query.SearchText)
                                     && prod.Store.CampusId == locationId
                                     && prod.Store.HostelId == subLocationId
                                     && prod.Price >= query.MinPrice
                                     && prod.Price <= query.MaxPrice;
                }

                if (locationId > 0 && subLocationId > 0)
                {
                    return (prod) => prod.CategoryId == Id && prod.Name.StartsWith(query.SearchText)
                                     && prod.Store.CampusId == locationId
                                     && prod.Store.HostelId == subLocationId
                                     && prod.Price >= query.MinPrice;
                }

                if (locationId > 0)
                {
                    return (prod) => prod.CategoryId == Id && prod.Name.StartsWith(query.SearchText)
                                     && prod.Store.CampusId == locationId
                                     && prod.Price >= query.MinPrice;
                }

                return (prod) => prod.CategoryId == Id && prod.Name.StartsWith(query.SearchText);
            }
            else
            {
                if (locationId > 0 && subLocationId > 0 && query.MaxPrice > 0)
                {
                    return (prod) => prod.CategoryId == Id 
                                     && prod.Store.CampusId == locationId
                                     && prod.Store.HostelId == subLocationId
                                     && prod.Price >= query.MinPrice
                                     && prod.Price <= query.MaxPrice;
                }

                if (locationId > 0 && subLocationId > 0)
                {
                    return (prod) => prod.CategoryId == Id
                                     && prod.Store.CampusId == locationId
                                     && prod.Store.HostelId == subLocationId
                                     && prod.Price >= query.MinPrice;
                }

                if (locationId > 0)
                {
                    return (prod) => prod.CategoryId == Id
                                     && prod.Store.CampusId == locationId
                                     && prod.Price >= query.MinPrice;
                }

            }

            return (prod) => prod.CategoryId == Id;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }
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