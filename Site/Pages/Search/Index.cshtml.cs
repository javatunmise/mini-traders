using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using site.Data;

namespace site.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly ISiteContentProvider _provider;

        public IEnumerable<Category> Categories { get; private set; }
        public IEnumerable<MarketLocation> Locations { get; private set; }
        public IEnumerable<MarketLocation> SubLocations { get; private set; }
        public SearchQuery Query { get; set; }

        public IndexModel(ISiteContentProvider provider)
        {
            _provider = provider;
        }


        public async Task OnGet([FromQuery] SearchQuery query)
        {
            //var location = 0;
            //var subLocation = 0;
            //var minPrice = 0M;
            //var maxPrice = 0M;
            //var categoryId = 0;
            var searchText = "";
            var pageIndex = 1;
            var pageSize = 25;

            Categories = (await _provider.GetAllCategories()).Where(c => c.Parent == null);
            Locations = await _provider.GetLocations();
            SubLocations = Locations.Where(x => x.Parent?.Id > 0 && x.Parent?.Id.ToString() == query.LocationId);
            Query = query;
        }

        public async Task<JsonResult> OnGetSubLocations(int curLocation)
        {
            var locations = await _provider.GetLocations();
            var subLocations = locations.Where(x => x.Parent?.Id > 0 && x.Parent?.Id == curLocation);

            var json = JsonConvert.SerializeObject(subLocations);
            return new JsonResult(json);
        }
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