using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class ProductsGrid : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ProductsGridInfo model)
        {
            await Task.CompletedTask;
            return View(model);
        }

        public class ProductsGridInfo
        {
            public string Title { get; set; }
        }
    }
}
