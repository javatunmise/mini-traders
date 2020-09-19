using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using site.Data;
using Microsoft.Extensions.Options;
using Shared.Entities;

namespace site.ViewComponents
{
    public class ProductsGrid : ViewComponent
    {
        private readonly List<Data.Category> _categories;
        private readonly ISiteContentProvider _provider;

        public ProductsGrid(IOptions<LatestProductsCategories> latestProductsCategories, ISiteContentProvider provider)
        {
            _categories = latestProductsCategories.Value.Categories;
            _provider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count = _categories.Count;
            var emptyProducts = new List<Shared.Entities.Product>();
            var model = new LatestProductsInfo
            {
                Categories = _categories,
                FirstCategoryProducts = count > 0 ? await _provider.GetLatestProducts(_categories[0].Id, 10) : emptyProducts,
                SecondCategoryProducts = count > 1 ? await _provider.GetLatestProducts(_categories[1].Id, 10) : emptyProducts,
                ThirdCategoryProducts = count > 2 ? await _provider.GetLatestProducts(_categories[2].Id, 10) : emptyProducts
            };

            await Task.CompletedTask;
            return View(model);
        }

        public class ProductsGridInfo
        {
            public string Title { get; set; }
        }

        public class LatestProductsInfo
        {
            public List<Data.Category> Categories { get; internal set; }
            public List<Product> FirstCategoryProducts { get; internal set; }
            public List<Product> SecondCategoryProducts { get; internal set; }
            public List<Product> ThirdCategoryProducts { get; internal set; }
        }
    }

    public class LatestProductsCategories
    {
        public string First { get; set; }
        public string Second { get; set; }
        public string Third { get; set; }
        private List<Data.Category> _categories = new List<Data.Category>();

        public LatestProductsCategories()
        {
        }

        public List<Data.Category> Categories
        {
            get
            {
                if(_categories.FirstOrDefault() == null)
                {
                    var split = First.Split(":");
                    _categories.Add(new Data.Category { Id = int.Parse(split[0]), Name = split[1] });

                    split = Second.Split(":");
                    _categories.Add(new Data.Category { Id = int.Parse(split[0]), Name = split[1] });

                    split = Third.Split(":");
                    _categories.Add(new Data.Category { Id = int.Parse(split[0]), Name = split[1] });
                }

                return _categories;
            }
        }
    }
}
