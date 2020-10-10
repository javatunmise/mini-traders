using Microsoft.AspNetCore.Mvc;
using site.Data;
using site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class ProductsGrid_Services : ViewComponent
    {
        private readonly ISiteContentProvider _provider;

        public ProductsGrid_Services(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Shared.Entities.Product> services = await _provider.GetTopServices();

            return View(services);
        }
    }

}