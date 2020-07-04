using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class LatestNews : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View();
        }

        public class ProductsGridInfo
        {
            public string Title { get; set; }
        }
    }
}
