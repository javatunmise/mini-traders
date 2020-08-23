using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Entities;
using Site.Data;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace site.Repositories
{
    public class AccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Account> FindByUsername(string username)
        {
            var user = await _context.SiteUsers.FirstOrDefaultAsync(f => f.Email == username);
            if (user == null)
            {
                _context.SiteUsers.Add(new SiteUser { Email = username });
                await _context.SaveChangesAsync();
            }

            var store = await _context.Stores.FirstOrDefaultAsync(f => f.SiteUserId == user.Id);

            var account = new Account(user);
            account.Store = AttachAssignedStore(account, store);

            return account;
        }

        private Shared.Store AttachAssignedStore(Account account, Shared.Entities.Store store)
        {
            if (store != null)
            {
                return new Shared.Store(account)
                {
                    Name = store.StoreName,
                    StoreDescription = store.StoreDescription,
                    CampusId = store.CampusId,
                    HostelId = store.HostelId,
                    LogoPath = store.LogoPath,
                    UploadedDocLocation = store.UploadedDocPath,
                    ReferrerCode = store.ReferrerCode,
                    PhoneNumber = store.PhoneNumber
                };
            }

            return null;
        }

        public Task<SiteUser> FindSiteUser(string username)
        {
           return _context.SiteUsers.FirstAsync(f => f.Email == username);
        }
    }
}
