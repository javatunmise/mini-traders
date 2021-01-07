using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Entities
{
    public class FlashDeal
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MinimumDiscount { get; set; }
        public FlashDealStatuses Status { get; set; }
        public void ExtendBy(int days) { EndDate.AddDays(days); }
        public void ExtendTill(DateTime futureDate) { if(futureDate > StartDate) EndDate = futureDate; }
    }

    public enum FlashDealStatuses
    {
        Inactive = 0,
        Active = 1
    }
    public class FlashDealProduct
    {
        public int FlashDealId { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public decimal OldPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Discount { get; set; }
        public FlashDeal FlashDeal { get; set; }
        public Product Product { get; set; }
    }
}
