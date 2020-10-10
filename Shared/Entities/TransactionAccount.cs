using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class TransactionAccount
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public SiteUser SiteUser { get; set; }
        public int SiteUserId { get; set; }
        public AccountTypes AccountType { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public enum AccountTypes
    {
        Wallet = 100, Token = 200
    }
}
