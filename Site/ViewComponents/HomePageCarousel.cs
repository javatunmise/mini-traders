using Microsoft.AspNetCore.Mvc;
using site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class HomePageCarousel : ViewComponent
    {
        private readonly ISiteContentProvider _provider;

        public HomePageCarousel(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _provider.GetCarousel();
            return View(data.Where(c => c.Type == CarouselType.MainScroller));
        }
    }
}
