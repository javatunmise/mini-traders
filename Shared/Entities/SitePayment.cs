using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class SitePayment
    {
        public string PaymentRef { get; set; }
        public decimal Amount { get; set; }
        public string EntityId { get; set; }
        public string EntityType { get;set; }
        public string PaymentDescription { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? PaymentDate { get; set; }
        public PaymentStatuses Status { get; set; }
        public string ExternalRef { get; set; }
        public string Email { get; set; }
    }


}

namespace Shared
{
    public enum PaymentStatuses
    {
        Initiated, Cancelled, Completed
    }
}