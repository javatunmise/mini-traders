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
            var account = new Visitor().SignUpAsSeller();
            var store = CreateStore(account);
        }

        private Store CreateStore(Account account)
        {
            if(account.IsActive)
            {
                return new Store();
            }

            throw new Exception("Store creation not permitted");
        }
    }

    class StoreManager
    {
        public Store AssignStore(Account account)
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

    class StoreOwner
    {
        Store Store { get; }
        Account Account { get; }
    }

    public class Seller
    {
        void createStore() { }
        void addItemToStore(Item item, Store store) { }
    }

    class Item
    {

    }
    class Store
    {
        public Account Owner { get; internal set; }
    }

    class Authority //Admin, System
    {

    }

    class Visitor
    {
        public Account SignUpAsSeller()
        {
            //An account is created
            //A store is given, in a specific location

            //verification of credentials before account is created: valid id
            return new Account();
        }

        public Account SignUpAsUser()
        {
            return new Account();
        }
    }

    class Account
    {
        public bool IsActive { get; internal set; }
        public int Id { get; internal set; }
    }
}
