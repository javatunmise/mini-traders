using Microsoft.AspNetCore.Mvc;
using site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class LocationMegaMenu : ViewComponent
    {
        private readonly ISiteContentProvider _siteContentProvider;

        public LocationMegaMenu(ISiteContentProvider provider)
        {
            _siteContentProvider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var locations = await _siteContentProvider.GetLocations();
            return View(locations);
        }
    }
}
