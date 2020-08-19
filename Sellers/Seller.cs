using System;
using System.Collections.Generic;
using System.Linq;

namespace Sellers
{
    class LoginFormSubmitted { }
    class SellerFormSubmitted { }
    class StoreCreated { }

    class FormSubmitted
    {
        void Execute()
        {
            //var account = new Visitor().SignUpAsSeller();
            //var store = CreateStore(account);
        }

        private Store CreateStore(Account account)
        {
            if(account.IsActive)
            {
                //return new Store();
            }

            throw new Exception("Store creation not permitted");
        }
    }

    class StoreManager
    {
        public Store AssignStore(Shared.Account account)
        {
            var stores = GetExistingStores(account.Id);
            if (stores.Any())
                throw new Exception("You already have a store");

            var store = new Store
            {
                Owner = account
            };

            return store;
        }

        private List<Store> GetExistingStores(int id)
        {
            return new List<Store>();
        }
    }

    public class Seller
    {
        void createStore() { }
        void addItemToStore(Item item, Store store) { }
    }

    class Item
    {

    }

    class Authority //Admin, System
    {

    }
}
