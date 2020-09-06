using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class Referral
    {
        public Referral()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int ReferrerUserId { get; set; }
        public string ReferralCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
