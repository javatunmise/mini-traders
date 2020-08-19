using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Repositories
{
    public class AccountRepository
    {
        public Task<Account> FindByUsername(string username)
        {
            return Task.FromResult(new Account());
        }
    }
}
