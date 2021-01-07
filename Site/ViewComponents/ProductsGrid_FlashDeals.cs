using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class ProductsGrid_FlashDeals : ViewComponent
    {
        private readonly ISiteContentProvider _provider;

        public ProductsGrid_FlashDeals(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var f = await _provider.GetCurrentFlashDeal();
            var dealProducts = new List<FlashDealProduct>();
            if (f != null) dealProducts = await _provider.GetFlashDeals(f.Id);

            var flashDeals = new FlashDealInfo
            {
                FlashDeal = f,
                FlashDealProducts = dealProducts
            };

            return View(flashDeals);
        }

        public class FlashDealInfo
        {
            public FlashDeal FlashDeal { get; set; }
            public List<FlashDealProduct> FlashDealProducts { get; set; }

            public (int Days, int Hours, int Minutes, int Seconds) TimeLeft()
            {
                var timespan = (DateTime.Now - FlashDeal.EndDate);
                var hasDays = timespan.Days > 0;
                var split = timespan.ToString().Split(':','.');
                if (hasDays)
                    return (int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));

                return (0, int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));
            }
        }
    }
}
