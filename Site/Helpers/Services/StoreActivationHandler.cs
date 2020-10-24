using Shared.Entities;
using site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Helpers.Services
{
    public class StoreActivationHandler
    {
        public ActivateStoreResult Handle(SitePayment payment, Store store, 
                                          RefererAccount referer)
        {
            if (payment.Amount <= 0) return null;
            if (payment.EntityType != "STORE") return null;
            if (store.Status == StoreStatuses.Active) return null;

            store.Status = StoreStatuses.Active;

            var transEntries = new List<TransactionEntry>();

            var shareAmount = payment.Amount - payment.Charge;
            if (referer != null && IsEligible(referer))
            {
                transEntries.Add(new CreditEntry(shareAmount * 0.25M,
                                                 referer.WalletAccountId.Value,
                                                 $"Bonus from referring {store.StoreName}"));

                transEntries.Add(new CreditEntry(shareAmount * 0.25M,
                                                 referer.TokenAccountId.Value,
                                                 $"Bonus from referring {store.StoreName}"));
            }

            var result = new ActivateStoreResult
            {
                Entries = transEntries,
                Store = store,
                Payment = payment
            };

            return result;
        }

        private bool IsEligible(RefererAccount referer)
        {
            return referer.Store.Status == StoreStatuses.Active &&
                   referer.WalletAccountId.HasValue && 
                   referer.TokenAccountId.HasValue;
        }
    }

    public class ActivateStoreResult
    {
        public List<TransactionEntry> Entries { get; set; }
        public Store Store { get; set; }
        public SitePayment Payment { get; set; }
    }
}
