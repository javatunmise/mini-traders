using System;

namespace Customers
{
    public class Customer
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

    public class Account
    {
    }
}
