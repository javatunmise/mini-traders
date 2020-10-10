using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class FlashDeal
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public void ExtendBy(int days) { }
        public void ExtendTill(DateTime futureDate) { }
    }

    public class FlashDealProduct
    {
        public int FlashDealId { get; set; }
        public int ProductId { get; set; }
        public FlashDeal FlashDeal { get; set; }
        public Product Product { get; set; }
    }
}
