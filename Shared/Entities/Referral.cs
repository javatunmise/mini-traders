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

    public class RefererAccount
    {
        public string FullName { get; set; }
        public Store Store
        {
            get
            {
                return new Store { Id = StoreId, Status = StoreStatus, StoreName = StoreName };
            }
        }
        public string ReferralCode { get; set; }
        public int? WalletAccountId { get; set; }
        public int? TokenAccountId { get; set; }
        public int StoreId { get; set; }
        public StoreStatuses StoreStatus { get; set; }
        public string StoreName { get; set; }
    }
}
