using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class StoreKeeper
    {
        public Store AssignStore(Account account, SellerRegistrationForm form)
        {
            if (account.HasStore)
                throw new InvalidOperationException("This account is already a seller on this site");

            var store = new Store(account)
            {
                Name = form.StoreName,
                StoreDescription = form.StoreDescription,
                UploadedDocLocation = form.DocumentLocation,
                ReferrerCode = form.ReferrerCode,
                CampusId = form.CampusId,
                HostelId = form.HostelId,
                PhoneNumber = form.PhoneNumber,
                LogoPath = "images/vendor-photo.png"
            };

            return store;
        }
    }
}
