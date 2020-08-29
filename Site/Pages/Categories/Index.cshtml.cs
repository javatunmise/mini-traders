using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Data;

namespace site.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ISiteContentProvider _provider;

        public IEnumerable<Category> Categories { get; private set; }
        public Category Category { get; set; }
        public IEnumerable<MarketLocation> Locations { get; private set; }
        public IEnumerable<MarketLocation> SubLocations { get; private set; }
        public SearchQuery Query { get; set; }

        public IndexModel(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        public async Task OnGet([FromQuery] SearchQuery query)
        {
            Categories = new List<Category>();
            if (Id > 0)
            {
                var _categories = await _provider.GetAllCategories();
                var _category = _categories.SingleOrDefault(c => c.Id == Id);

                Category = _category ?? new Category();
                Categories = _categories.Where(c => c.Parent?.Id == Id);
            }

            int.TryParse(query.LocationId, out int locationId);
            Locations = await _provider.GetLocations();
            SubLocations = await _provider.GetSubLocations(locationId);
            Query = query;
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
        public string MinPrice { get; set; }

        [FromQuery(Name = "max_price")]
        public string MaxPrice { get; set; }

        [FromQuery(Name = "q")]
        public string SearchText { get; set; }
    }
}