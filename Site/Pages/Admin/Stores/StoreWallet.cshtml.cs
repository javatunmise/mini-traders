using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using site.Repositories;
using Site.Data;

namespace site.Pages.Admin.Stores
{
    public class StoreWalletModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public StoreWalletModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Store CurrentStore { get; private set; }
        public TransactionAccount WalletAccount { get; private set; }
        public decimal WalletBalance { get; private set; }
        public List<TransactionEntry> TransHistories { get; private set; }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentStore = await context.Stores.AsNoTracking()
                                 .Include(x => x.User)
                                 .FirstOrDefaultAsync(x => x.Id == id);

            WalletAccount = context.TransactionAccounts.SingleOrDefault(e => e.AccountId == CurrentStore.User.WalletAccountCode);

            if (WalletAccount != null)
            {
                TransHistories = await context.TransactionEntries.AsNoTracking()
                                .Where(e => e.TransactionAccountId == WalletAccount.Id)
                                .OrderByDescending(e => e.CreatedOn)
                                .ToListAsync();

                WalletBalance = TransHistories.Sum(e => e.Amount);
            }

            return Page();
        }
    }
}
