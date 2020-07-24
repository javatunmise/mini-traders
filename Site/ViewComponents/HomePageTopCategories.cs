using Microsoft.AspNetCore.Mvc;
using site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class HomePageTopCategories : ViewComponent
    {
        private readonly ISiteContentProvider provider;

        public HomePageTopCategories(ISiteContentProvider provider)
        {
            this.provider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            var categories = await provider.GetSiteTopCategories();
            return View(categories);
        }
    }
}
