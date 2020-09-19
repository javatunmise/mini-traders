using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class Account
    {
        private SiteUser _user;

        public Account(SiteUser user)
        {
            _user = user;
            Id = _user.Id;
        }

        public bool IsAdmin { get; set; }
        public bool IsActive { get; internal set; }
        public int Id { get; }
        public bool HasStore
        {
            get { return Store != null; }
        }

        public Store Store { get; set; }

        public static Account NotCreatedAccount
        {
            get { return new Account(null); }
        }
    }
}
