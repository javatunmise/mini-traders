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

        public IEnumerable<MarketLocation> Locations { get; private set; }

        public IndexModel(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        public async Task OnGet()
        {
            Locations =  await _provider.GetLocations();
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }
    }
}