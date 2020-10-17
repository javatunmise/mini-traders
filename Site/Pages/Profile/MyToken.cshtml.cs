using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using site.Helpers;
using site.Repositories;
using Site.Data;

namespace site.Pages.Profile
{
    public class MyTokenModel : PageModel
    {
        private readonly AccountRepository _accountRepo;
        private readonly ApplicationDbContext _dbContext;
        private readonly StoreRepository _storeRepo;

        public MyTokenModel(AccountRepository accountRepository,
                            StoreRepository storeRepository,
                            ApplicationDbContext context)
        {
            _accountRepo = accountRepository;
            _dbContext = context;
            _storeRepo = storeRepository;
        }

        public TransactionAccount TokenAccount { get; private set; }
        public decimal TokenBalance { get; private set; }
        public List<TransactionEntry> TransHistories { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            var currentUser = await _accountRepo.FindByUsername(User.Identity.Name);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var accounts = await _dbContext.TransactionAccounts.AsNoTracking()
                                 .Where(e => e.SiteUserId == currentUser.Id)
                                 .ToListAsync();

            if (accounts.Count == 0)
            {
                var tokenAccountId = StringUtil.GenerateTokenAccountId(currentUser.Store.Id, currentUser.Id);
                var walletAccountId = StringUtil.GenerateWalletAccountId(currentUser.Store.Id, currentUser.Id);

                await _storeRepo.CreateAccounts(currentUser.Id, walletAccountId, tokenAccountId);
                accounts.Add(new TransactionAccount { AccountType = AccountTypes.Token, AccountId = tokenAccountId });
            }

            TokenAccount = accounts.SingleOrDefault(e => e.AccountType == AccountTypes.Token);

            if (TokenAccount != null && TokenAccount.Id > 0)
            {
                TransHistories = await _dbContext.TransactionEntries
                                        .Where(e => e.TransactionAccountId == TokenAccount.Id)
                                        .OrderByDescending(e => e.CreatedOn)
                                        .ToListAsync();

                TokenBalance = TransHistories.Sum(e => e.Amount);
            }

            return Page();
        }
    }
}