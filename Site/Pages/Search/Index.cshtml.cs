using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Data;

namespace site.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly ISiteContentProvider _provider;

        public IEnumerable<Category> Categories { get; private set; }
        public IEnumerable<MarketLocation> Locations { get; private set; }

        public IndexModel(ISiteContentProvider provider)
        {
            _provider = provider;
        }


        public async Task OnGet([FromQuery] SearchQuery query)
        {
            var location = 0;
            var minPrice = 0M;
            var maxPrice = 0M;
            var categoryId = 0;
            var searchText = "";
            var pageIndex = 1;
            var pageSize = 25;

            Categories = (await _provider.GetAllCategories()).Where(c => c.Parent == null);
            Locations = await _provider.GetLocations();
        }
    }

    public class SearchQuery
    {
        [FromQuery(Name = "cat")]
        public string CategoryId { get; set; }

        [FromQuery(Name = "min-price")]
        public string MinPrice { get; set; }

        [FromQuery(Name = "max-price")]
        public string MaxPrice { get; set; }

        [FromQuery(Name = "q")]
        public string SearchText { get; set; }
    }
}