using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Entities;
using site.Helpers.Services;
using Site.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using site.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace site.Repositories
{
    public class StoreRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentDate _serverDate;
        private readonly IConfiguration _configuration;

        public StoreRepository(ApplicationDbContext context, ICurrentDate serverDate, IConfiguration configuration)
        {
            _context = context;
            _serverDate = serverDate;
            _configuration = configuration;
        }

        public async Task Create(Shared.Store store)
        {
            var campus = _context.Campuses.Find(store.CampusId);

            EnsureNonNull(campus, "Campus");

            Hostel hostel = null;
            if (store.HostelId > 0)
            {
                hostel = _context.Hostels.Find(store.HostelId);
                EnsureNonNull(hostel, "Hostel");
            }

            var _store = new Shared.Entities.Store()
            {
                Campus = campus,
                Hostel = hostel,
                StoreName = store.Name,
                StoreDescription = store.StoreDescription,
                ReferrerCode = store.ReferrerCode,
                PhoneNumber = store.PhoneNumber,
                LogoPath = store.LogoPath,
                UploadedDocPath = store.UploadedDocLocation,
                SiteUserId = store.Owner.Id
            };

            if (!string.IsNullOrWhiteSpace(store.ReferrerCode))
            {
                var referrerUser = await _context.SiteUsers.FirstOrDefaultAsync(e => e.ReferralCode == store.ReferrerCode);
                if (referrerUser != null)
                {
                    _context.Referrals.Add(new Referral
                    {
                        ReferrerUserId = referrerUser.Id,
                        Store = _store,
                        ReferralCode = referrerUser.ReferralCode,
                    });
                }
            }

            _context.Stores.Add(_store);

            await _context.SaveChangesAsync();
        }

        internal async Task SaveActivationResult(ActivateStoreResult result)
        {
            if (result == null) return;

            var store = await _context.Stores.FirstAsync(e => e.Id == result.Store.Id);
            store.Status = StoreStatuses.Active;
            store.ActivatedOn = _serverDate.Now();

            foreach(var tran in result.Entries)
            {
                _context.TransactionEntries.Add(tran);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Shared.Entities.Store> GetStoreById(int storeId)
        {
            var _store = await _context.Stores.FirstOrDefaultAsync(_ => _.Id == storeId);

            return _store;
        }
        public async Task<RefererAccount> GetRefererStoreById(int storeId)
        {
            var referral = await _context.Referrals.AsNoTracking().Include(e => e.Store).FirstOrDefaultAsync(_ => _.StoreId == storeId);
            if (referral == null) return null;

            var query = @"
            SELECT
	            u.Id SiteUserId,
	            ISNULL(u.FirstName,'') + ISNULL(' '+u.LastName,'') As FullName,
	            token.Id TokenAccountId,
	            wallet.Id WalletAccountId,
				sto.Id as StoreId,
				sto.StoreName,
				sto.[Status] as StoreStatus
            FROM siteusers u
			JOIN Stores sto ON sto.SiteUserId = u.id
            LEFT JOIN TransactionAccounts wallet ON wallet.SiteUserId = u.Id AND wallet.AccountType = 100
            LEFT JOIN TransactionAccounts token ON token.SiteUserId = u.Id AND token.AccountType = 200
            WHERE u.Id = @userid";

            using var conn = new SqlConnection(Config.GetConnectionString(_configuration));
            conn.Open();
            var referer = await conn.QueryFirstAsync<RefererAccount>(query, new { userid = referral.ReferrerUserId });
            return referer;
        }

        public Task Update(Shared.Store updatedStore)
        {
            var _store = _context.Stores.FirstOrDefault(_ => _.SiteUserId == updatedStore.Owner.Id);

            _store.StoreName = updatedStore.Name;
            _store.StoreDescription = updatedStore.StoreDescription;
            _store.PhoneNumber = updatedStore.PhoneNumber;
            _store.HostelId = updatedStore.HostelId ?? 0;
            _store.CampusId = updatedStore.CampusId;

            if (!string.IsNullOrEmpty(updatedStore.LogoPath))
                _store.LogoPath = updatedStore.LogoPath;

            return _context.SaveChangesAsync();
        }

        internal async Task CreateAccounts(int userId, string WalletAccountId, string TokenAccountId)
        {
            var user = await _context.SiteUsers.SingleAsync(e => e.Id == userId);
            user.TokenAccountCode = TokenAccountId;
            user.WalletAccountCode = WalletAccountId;

            var tkn = _context.TransactionAccounts.Add(new TransactionAccount
            {
                AccountId = TokenAccountId,
                AccountType = AccountTypes.Token,
                CreatedOn = _serverDate.Now(),
                SiteUserId = userId
            });

            var wlt = _context.TransactionAccounts.Add(new TransactionAccount
            {
                AccountId = WalletAccountId,
                AccountType = AccountTypes.Wallet,
                CreatedOn = _serverDate.Now(),
                SiteUserId = userId
            });

            await _context.SaveChangesAsync();
        }

        private void EnsureNonNull(object o, string paramName)
        {
            if (o == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
