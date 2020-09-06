using Microsoft.AspNetCore.Mvc;
using site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class AllCategoriesMenu : ViewComponent
    {
        private readonly ISiteContentProvider _siteContentProvider;

        public AllCategoriesMenu(ISiteContentProvider siteContentProvider)
        {
            _siteContentProvider = siteContentProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = (await _siteContentProvider.GetAllCategories());

            foreach(var c in categories)
            {
                c.Children = categories.Where(ch => ch.Parent?.Id == c.Id);
            }

            return View(categories);
        }
    }
}
