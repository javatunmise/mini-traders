using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class ProductsGrid_Services : ViewComponent
    {


        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;

            return View();
        }
    }

}