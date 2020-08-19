using Shared;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Sellers
{
    public class Account
    {
        public bool IsActive { get; internal set; }
        public int Id { get; internal set; }

        public Store RegisterAsSeller(SellerRegistrationForm form)
        {
            //if (form.HasNoStudentId)
            //    ValidateUsingBioData();
            //else
            //    ValidateUsingStudentId();

            var store = new Store(null);

            return store;
        }

        private void ValidateUsingBioData()
        {
            throw new NotImplementedException();
        }

        private void ValidateUsingStudentId()
        {
            throw new NotImplementedException();
        }

    }
}
